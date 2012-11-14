using System.Runtime.Serialization;

namespace Myers.NovCodeCamp.Contract
{
    public class SocketServiceMessage
    {
        public SocketServiceAction Action { get; set; }
        public string Username { get; set; }
        public ChatMessage Message { get; set; }
    }
}
