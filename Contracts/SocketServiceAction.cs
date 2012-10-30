using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Myers.NovCodeCamp.Contract
{
    [DataContract]
    public enum SocketServiceAction
    {
        [EnumMember]
        Login,

        [EnumMember]
        Logout,

        [EnumMember]
        ChatMessage
    }
}
