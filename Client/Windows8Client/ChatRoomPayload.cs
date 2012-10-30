using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Myers.NovCodeCamp.Client.Windows8.ChatServiceReference;

namespace Myers.NovCodeCamp.Client.Windows8
{
    public class ChatRoomPayload
    {
        public ChatRoomPayload(string username, ChatServiceClient chatServiceClient)
        {
            Username = username;
            ChatServiceClient = chatServiceClient;
        }

        public string Username { get; set; }
        public ChatServiceClient ChatServiceClient { get; set; }
    }
}
