using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Myers.WebSockDemo.Service
{
    [ServiceContract]
    public interface ISocketChatServiceCallback
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void Send(Message message);
    }

    [ServiceContract(CallbackContract = typeof(ISocketChatServiceCallback), SessionMode = SessionMode.Required)]
    public interface ISocketChatService
    {
        [OperationContract(IsOneWay = true, Action = "*")]
        void Receive(Message message);
    }
}
