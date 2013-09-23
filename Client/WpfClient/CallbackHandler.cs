using Myers.WebSockDemo.Contract;
using Myers.WebSockDemo.Service;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Myers.WebSockDemo.Client.Wpf
{
    public class CallbackHandler : ISocketChatServiceCallback
    {
        private ChatRoom _chatRoomWindow;

        public CallbackHandler(ChatRoom chatRoomWindow)
        {
            _chatRoomWindow = chatRoomWindow;
        }

        public void RecieveMessage(ChatMessage msg)
        {
            // create the paragraph
            Paragraph p = new Paragraph();
            Run rnMyText = new Run();
            p.FontWeight = FontWeights.Bold;

            // if the message is from the currently logged in user, then set the color to gray
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

            // add the text to the paragraph tag
            p.Inlines.Add(rnMyText);

            // add the paragraph to the rich text box
            _chatRoomWindow.rtbChatLog.Document.Blocks.Add(p);
        }

        public void Send(Message message)
        {
            var msg = JsonMessageSerializer.Deserialize<ChatMessage>(message);
            RecieveMessage(msg);
        }
    }
}
