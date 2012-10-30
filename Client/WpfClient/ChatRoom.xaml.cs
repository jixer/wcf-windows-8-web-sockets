using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Myers.NovCodeCamp.Client.Wpf.ChatServiceReference;

namespace Myers.NovCodeCamp.Client.Wpf
{
    /// <summary>
    /// Interaction logic for ChatRoom.xaml
    /// </summary>
    public partial class ChatRoom : Window
    {
        private ChatServiceClient _svcClient;

        public string Username { get; set; }

        public ChatRoom(string username)
        {
            InitializeComponent();
            Username = username;
            InstanceContext chatServiceCallbackInstance = new InstanceContext(new CallbackHandler(this));
            _svcClient = new ChatServiceClient(chatServiceCallbackInstance);
            _svcClient.Login(Username);
        }

        private void btnSendChat_Click(object sender, RoutedEventArgs e)
        {
            var msg = new ChatMessage() { From = Username, MessageText = txtUserChatMessage.Text};
            _svcClient.SendMessage(msg);
        }
    }

    public class CallbackHandler : IChatServiceCallback
    {
        private ChatRoom _chatRoomWindow;

        public CallbackHandler(ChatRoom chatRoomWindow)
        {
            _chatRoomWindow = chatRoomWindow;
        }

        public void RecieveMessage(ChatMessage msg)
        {
            Paragraph p = new Paragraph();
            Run rnMyText = new Run();
            
            
            p.FontWeight = FontWeights.Bold;
            if (msg.From == _chatRoomWindow.Username)
            {
                p.Foreground = new SolidColorBrush(Colors.Gray);
                rnMyText.Text = string.Format("{0} (me): {1}", msg.From, msg.MessageText);
            }
            else
            {
                p.Foreground = new SolidColorBrush(Colors.Green); 
                rnMyText.Text = string.Format("{0}: {1}", msg.From, msg.MessageText);
            }

            p.Inlines.Add(rnMyText);
            _chatRoomWindow.rtbChatLog.Document.Blocks.Add(p);
        }
    }
}
