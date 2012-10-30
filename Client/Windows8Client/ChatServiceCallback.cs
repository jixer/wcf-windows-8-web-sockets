using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Myers.NovCodeCamp.Client.Windows8.ChatServiceReference;

namespace Myers.NovCodeCamp.Client.Windows8
{
    public class ChatServiceProxy : IChatServiceCallback
    {
        private ChatServiceClient _client;
        public ChatServiceClient Client
        {
            get
            {
                return _client;
            }
        }

        private Frame _appFrame;

        public ChatServiceProxy(ChatServiceClient client, Frame appFrame)
        {
            _client = client;
            _appFrame = appFrame;
        }

        public void Login(string username)
        {

        }

        public void RecieveMessage(ChatMessage msg)
        {
            
        }
    }
}
