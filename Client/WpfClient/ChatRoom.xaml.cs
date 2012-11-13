using System.ServiceModel;
using System.Windows;
using Myers.NovCodeCamp.Service;
using Myers.NovCodeCamp.Contract;

namespace Myers.NovCodeCamp.Client.Wpf
{
    /// <summary>
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoom : Window
    {
        // global chat service reference
        private ISocketChatService _svcClient;

        /// <summary>
        /// Public access to the logged in user
        /// </summary>
        public string Username { get; set; }


        public ChatRoom(string username)
        {
            InitializeComponent();
            Username = username;

            // create the callback handler and the service client
            var callbackContext = new InstanceContext(new CallbackHandler(this));
            var factory = new DuplexChannelFactory<ISocketChatService>(callbackContext, "SocketChatService");
            _svcClient = factory.CreateChannel();

            // Create the custom logon message and send in order to logon
            var loginMessageBody = new SocketServiceMessage() { Action = SocketServiceAction.Login, Username = username };
            var loginMessage = JsonMessageSerializer.Serialize(loginMessageBody);
            _svcClient.Receive(loginMessage);
        }

        /// <summary>
        /// Handle Chat Message button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendChat_Click(object sender, RoutedEventArgs e)
        {
            //// create the chat message
            var chatMessageBody = new ChatMessage() { From = Username, MessageText = txtUserChatMessage.Text };
            var messageBody = new SocketServiceMessage() { Action = SocketServiceAction.ChatMessage, Message = chatMessageBody };
            var msg = JsonMessageSerializer.Serialize(messageBody);

            ////send the message to the service topic
            _svcClient.Receive(msg);
        }
    }
}
