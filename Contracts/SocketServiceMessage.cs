using System.Runtime.Serialization;

namespace Myers.WebSockDemo.Contract
{
    [DataContract]
    public class SocketServiceMessage
    {
        [DataMember]
        public SocketServiceAction Action { get; set; }

        [DataMember]
        public string Username { get; set; }

        [DataMember]
        public ChatMessage Message { get; set; }
    }
}
