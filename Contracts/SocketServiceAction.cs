using System.Runtime.Serialization;

namespace Myers.WebSockDemo.Contract
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
