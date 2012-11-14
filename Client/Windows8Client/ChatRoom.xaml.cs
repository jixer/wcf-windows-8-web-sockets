using System;
using System.Collections.Generic;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Web;
using Myers.NovCodeCamp.Contract;
using Newtonsoft.Json;

namespace Myers.NovCodeCamp.Client.Windows8
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class ChatRoom : Myers.NovCodeCamp.Client.Windows8.Common.LayoutAwarePage
    {
        private string _username;

        private MessageWebSocket messageWebSocket;
        private DataWriter messageWriter;

        private Queue<ChatMessage> _queue = new Queue<ChatMessage>();
        private object _queueSync = new object();

        public ChatRoom()
        {
            this.InitializeComponent();
        }

        private ChatMessage DequeueChatMessage()
        {
            lock (_queueSync)
            {
                if (_queue.Count > 0) return _queue.Dequeue();
                else return null;
            }
        }

        private void DisplayMessage()
        {
            ChatMessage msg = null;
            while ((msg = DequeueChatMessage()) != null)
            {
                // create the paragraph
                Paragraph p = new Paragraph();
                Run rnMyText = new Run();
                p.FontWeight = FontWeights.Bold;

                // if the message is from the currently logged in user, then set the color to gray
                if (msg.From == _username)
                {
                    p.Foreground = new SolidColorBrush(Colors.Gray);
                    rnMyText.Text = string.Format("{0} (me): {1}", msg.From, msg.MessageText);
                }
                else
                {
                    p.Foreground = new SolidColorBrush(Colors.Green);
                    rnMyText.Text = string.Format("{0}: {1}", msg.From, msg.MessageText);
                }

                // add the text to the paragraph tag
                p.Inlines.Add(rnMyText);

                // add the paragraph to the rich text box
                rtbChatLog.Blocks.Add(p);
            }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState) { }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // retrieve the username from the payload
            _username = (string)e.Parameter;

            try
            {
                // Make a local copy to avoid races with Closed events.
                MessageWebSocket webSocket = messageWebSocket;

                // Have we connected yet?
                if (webSocket == null)
                {
                    Uri server = new Uri("ws://vdev-pc/svc/SocketChatService.svc");

                    webSocket = new MessageWebSocket();
                    // MessageWebSocket supports both utf8 and binary messages.
                    // When utf8 is specified as the messageType, then the developer
                    // promises to only send utf8-encoded data.
                    webSocket.Control.MessageType = SocketMessageType.Utf8;
                    // Set up callbacks
                    webSocket.MessageReceived += MessageReceived;

                    await webSocket.ConnectAsync(server);
                    messageWebSocket = webSocket; // Only store it after successfully connecting.
                    messageWriter = new DataWriter(webSocket.OutputStream);
                }

                // create the main message and serialize to string
                SocketServiceMessage msgBody = new SocketServiceMessage() { Action = SocketServiceAction.Login, Username = _username };
                string msgBodyString = JsonConvert.SerializeObject(msgBody);

                // Buffer any data we want to send.
                messageWriter.WriteString(msgBodyString);

                // Send the data as one complete message.
                await messageWriter.StoreAsync();
            }
            catch (Exception ex) // For debugging
            {
                WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
            }
        }

        private void MessageReceived(MessageWebSocket sender, MessageWebSocketMessageReceivedEventArgs args)
        {
            ChatMessage msg = null;
            try
            {
                using (DataReader reader = args.GetDataReader())
                {
                    // get message and deserialize to ChatMessage
                    reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
                    string read = reader.ReadString(reader.UnconsumedBufferLength);
                    msg = JsonConvert.DeserializeObject<ChatMessage>(read);
                }
            }
            catch (Exception ex) // For debugging
            {
                WebErrorStatus status = WebSocketError.GetStatus(ex.GetBaseException().HResult);
                // Add your specific error-handling code here.
            }

            // Add item to the local queue
            _queue.Enqueue(msg);

            // Dispatch the queue handler
            this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, DisplayMessage);
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState) { }

        /// <summary>
        /// Handle the user clicking the send button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnSendChatMessage_Click(object sender, RoutedEventArgs e)
        {
            // create the chat message and send it across the wire
            var chatMessage = new ChatMessage() { From = _username, MessageText = txtUserMessage.Text };

            // create the wrapper message and serialize it to string
            var wrapperMsg = new SocketServiceMessage() { Action = SocketServiceAction.ChatMessage, Message = chatMessage };
            string msg = JsonConvert.SerializeObject(wrapperMsg);

            // buffer the message
            messageWriter.WriteString(msg);

            // send the message
            await messageWriter.StoreAsync();

            //_svcClient.SendMessageAsync(msg);

            // clear out the message text box
            txtUserMessage.Text = "";
        }
    }
}
