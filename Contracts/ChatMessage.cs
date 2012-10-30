using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Myers.NovCodeCamp.Contract
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
