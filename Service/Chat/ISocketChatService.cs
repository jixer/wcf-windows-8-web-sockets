using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Myers.NovCodeCamp.Service
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
