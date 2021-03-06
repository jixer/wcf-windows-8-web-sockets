﻿using Myers.WebSockDemo.Contract;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;

namespace Myers.WebSockDemo.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class SocketChatService : ISocketChatService
    {
        #region Constructor / Destructor

        ~SocketChatService()
        {
            Logout();
        }

        #endregion

        #region Private Members

        // chat topic name
        private const string ChatMessageExchange = "chat_message";

        // local references for queue and queue connection
        private IConnection con;
        private IModel model;

        // local reference to logged in user for the session (PerSession call)
        private string username;

        private string queueName;
        private bool receiveMessages = true;

        /// <summary>
        /// Callback for the client (duplex service)
        /// </summary>
        private ISocketChatServiceCallback Callback
        {
            get { return OperationContext.Current.GetCallbackChannel<ISocketChatServiceCallback>(); }
        }

        #endregion

        #region ISocketChatService Implementors

        public void Receive(Message message)
        {
            if (!message.IsEmpty) // keep alives and connects hit this operation also
            {
                // Deserialization does not just convert JsonString to bytes
                SocketServiceMessage msg = JsonMessageSerializer.Deserialize<SocketServiceMessage>(message);

                // which operation is desired?
                switch (msg.Action)
                {
                    case SocketServiceAction.Login:
                        // kickoff the login process
                        this.Login(msg.Username);
                        break;
                    case SocketServiceAction.Logout: // user may not explicitly request logout
                        this.Logout();
                        break;
                    case SocketServiceAction.ChatMessage:
                        // Send the message to the topic
                        this.SendMessage(msg.Message);
                        break;
                    default:
                        break;
                }
            }

        }

        #endregion

        #region IChatService Implementors


        public void SendMessage(ChatMessage msg)
        {
            // deserialize the message to the buffer
            byte[] buf;
            using (var memStream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(memStream, msg);
                memStream.Seek(0, SeekOrigin.Begin);
                buf = new byte[memStream.Length];
                memStream.Read(buf, 0, (int)memStream.Length);
            }

            // Broadcast the chat message through the topic
            model.BasicPublish(ChatMessageExchange, "chat", null, buf);
        }

        public void Login(string username)
        {
            // mark the service with this session's logged in user
            this.username = username;

            // before doing anything, register for the client connection closed event
            OperationContext.Current.InstanceContext.Closing += (sender, e) => { Logout(); };

            // establish the rabbitMQ connection
            var cf = new ConnectionFactory();
            con = cf.CreateConnection();
            model = con.CreateModel();

            // establish the rabbitMQ queue
            var queue = model.QueueDeclare();
            queueName = queue.QueueName;

            // establish the rabbitMQ topic
            model.ExchangeDeclare(ChatMessageExchange, "topic");

            // establish the subscription to the topic
            model.QueueBind(queueName, ChatMessageExchange, "*");

            // kick off the asynchronous recieving process
            var action = new Action<ISocketChatServiceCallback>(Subscribe);
            var asyncResult = action.BeginInvoke(Callback, SubscribeCallback, Callback);
        }

        public void Logout()
        {
            // set the session for the service to null
            receiveMessages = false;
            username = "";

            // close the queue and model if they are open
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

        protected void Subscribe(ISocketChatServiceCallback callback)
        {
            // receive a message from the topic if available
            var result = model.BasicGet(queueName, true);
            if (result != null)
            {
                // if message was available, deserialize it to a ChatMessage
                ChatMessage msg;
                using (var memStrm = new MemoryStream(result.Body))
                {
                    var serializer = new BinaryFormatter();
                    msg = (ChatMessage)serializer.Deserialize(memStrm);
                }

                // send the chat message to the client
                callback.Send(JsonMessageSerializer.Serialize(msg));
            }
            else
            {
                // sleep for half a second before retrieving more messages
                Thread.Sleep(500);
            }
        }

        protected void SubscribeCallback(IAsyncResult result)
        {
            // verify that session has not ended
            if (receiveMessages)
            {
                // continue receiving messages
                var ctx = (ISocketChatServiceCallback)result.AsyncState;
                var action = new Action<ISocketChatServiceCallback>(Subscribe);
                action.BeginInvoke(ctx, SubscribeCallback, ctx);
            }
        }

        #endregion
    }
}
