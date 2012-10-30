using Myers.NovCodeCamp.Contract;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Myers.NovCodeCamp.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class ChatService : IChatService
    {
        #region Constructor / Destructor

        ~ChatService()
        {
            Logout();
        }

        #endregion

        #region Private Members

        private string username;
        private const string ChatMessageExchange = "chat_message";
        private IConnection con;
        private IModel model;
        private string queueName;
        private bool receiveMessages = true;

        private IChatServiceCallback Callback
        {
            get { return OperationContext.Current.GetCallbackChannel<IChatServiceCallback>(); }
        }

        #endregion

        #region IChatService Implementors

        public void SendMessage(ChatMessage msg)
        {
            if (!string.IsNullOrEmpty(username))
            {
                byte[] buf;
                using (var memStream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(memStream, msg);
                    memStream.Seek(0, SeekOrigin.Begin);
                    buf = new byte[memStream.Length];
                    memStream.Read(buf, 0, (int)memStream.Length);
                }

                model.BasicPublish(ChatMessageExchange, "chat", null, buf);
            }
        }

        public void Login(string username)
        {
            this.username = username;
            var cf = new ConnectionFactory();
            con = cf.CreateConnection();
            model = con.CreateModel();
            var queue = model.QueueDeclare();
            queueName = queue.QueueName;
            model.ExchangeDeclare(ChatMessageExchange, "topic");
            model.QueueBind(queueName, ChatMessageExchange, "*");

            var action = new Action<IChatServiceCallback>(Subscribe);
            var asyncResult = action.BeginInvoke(Callback, SubscribeCallback, Callback);
            OperationContext.Current.InstanceContext.Closing += (sender, e) => { Logout(); };
        }

        public void Logout()
        {
            receiveMessages = false;
            username = "";

            if (model != null)
            {
                if (model.IsOpen) model.Close();
                model.Dispose();
                model = null;
            }

            if (con != null)
            {
                if (con.IsOpen) con.Close();
                con.Dispose();
                con = null;
            }
        }

#endregion

        #region Async Handlers

        protected void Subscribe(IChatServiceCallback callback)
        {            
            var result = model.BasicGet(queueName, true);
            if (result != null)
            {
                ChatMessage msg;
                using (var memStrm = new MemoryStream(result.Body))
                {
                    var serializer = new BinaryFormatter();
                    msg = (ChatMessage)serializer.Deserialize(memStrm);
                }
                
                callback.RecieveMessage(msg);
            }
            else
            {
                Thread.Sleep(500);
            }
        }

        protected void SubscribeCallback(IAsyncResult result)
        {
            if (receiveMessages)
            {
                var ctx = (IChatServiceCallback)result.AsyncState;
                var action = new Action<IChatServiceCallback>(Subscribe);
                action.BeginInvoke(ctx, SubscribeCallback, ctx);
            }
        }

        #endregion
    }
}