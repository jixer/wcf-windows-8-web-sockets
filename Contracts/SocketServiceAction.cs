using System.Runtime.Serialization;

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
