using Myers.NovCodeCamp.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Myers.NovCodeCamp.Service
{
    [ServiceContract(CallbackContract = typeof(IChatServiceCallback), SessionMode = SessionMode.Required)]
    public interface IChatService
    {
        [OperationContract(IsOneWay = true)]
        void Login(string username);

        [OperationContract(IsOneWay = true)]
        void SendMessage(ChatMessage msg);

        [OperationContract(IsOneWay = true)]
        void Logout();
    }

    [ServiceContract]
    public interface IChatServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void RecieveMessage(ChatMessage msg);
    }
}
