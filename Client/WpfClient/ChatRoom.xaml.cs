using System.ServiceModel;
using System.Windows;
using Myers.NovCodeCamp.Client.Wpf.ChatServiceReference;

namespace Myers.NovCodeCamp.Client.Wpf
{
    /// <summary>
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoom : Window
    {
        // global chat service reference
        private ChatServiceClient _svcClient;

        /// <summary>
        /// Public access to the logged in user
        /// </summary>
        public string Username { get; set; }


        public ChatRoom(string username)
        {
            InitializeComponent();
            Username = username;

            // create the callback handler and the service client
            InstanceContext chatServiceCallbackInstance = new InstanceContext(new CallbackHandler(this));
            _svcClient = new ChatServiceClient(chatServiceCallbackInstance);

            // login to the chat service
            _svcClient.Login(Username);
        }

        /// <summary>
        /// Handle Chat Message button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendChat_Click(object sender, RoutedEventArgs e)
        {
            // create the chat message
            var msg = new ChatMessage() { From = Username, MessageText = txtUserChatMessage.Text};

            //send the message to the service topic
            _svcClient.SendMessage(msg);
        }
    }
}
