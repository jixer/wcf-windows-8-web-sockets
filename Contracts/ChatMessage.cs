using System;
using System.Runtime.Serialization;

namespace Myers.WebSockDemo.Contract
{
    [DataContract]
    [Serializable]
    public class ChatMessage
    {
        [DataMember]
        public string From { get; set; }

        [DataMember]
        public string MessageText { get; set; }
    }
}
