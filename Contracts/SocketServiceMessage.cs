using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Myers.NovCodeCamp.Contract
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
