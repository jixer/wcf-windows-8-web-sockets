using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Extensibility
{
    class BinaryWebSocketServiceActionExtracter : IDispatchMessageInspector, IClientMessageInspector
    {
        // duplex service recieve
        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            var copy = request.CreateBufferedCopy(int.MaxValue);
            var msg = copy.CreateMessage();
            var wrapper = msg.GetBody<MessageWrapper>();
            request = Message.CreateMessage(MessageVersion.Default, wrapper.Action, wrapper.Message);
            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }

        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        // duplex service callback send
        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            var copy = request.CreateBufferedCopy(int.MaxValue);
            var originalMsg = copy.CreateMessage();
            var newMessageBody = new MessageWrapper();
            newMessageBody.Action = originalMsg.Headers.Action;
            newMessageBody.Message = originalMsg.GetBody<byte[]>();
            request = Message.CreateMessage(MessageVersion.Default, "", newMessageBody);
            return null;
        }
    }
}
