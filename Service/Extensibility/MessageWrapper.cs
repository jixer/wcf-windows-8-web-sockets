using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Extensibility
{
    [DataContract]
    public class MessageWrapper
    {
        [DataMember]
        public string Action { get; set; }

        [DataMember]
        public byte[] Message { get; set; }
    }
}
