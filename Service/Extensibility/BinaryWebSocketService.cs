﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace Extensibility
{
    public class BinaryWebSocketService : Attribute, IServiceBehavior, IEndpointBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher channelDispatcher in serviceHostBase.ChannelDispatchers)
            {
                foreach (var ep in channelDispatcher.Endpoints)
                {
                    ep.DispatchRuntime.MessageInspectors.Add(new BinaryWebSocketServiceActionExtracter());
                    ep.DispatchRuntime.CallbackClientRuntime.ClientMessageInspectors.Add(new BinaryWebSocketServiceActionExtracter());
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            endpointDispatcher.DispatchRuntime.MessageInspectors.Add(new BinaryWebSocketServiceActionExtracter());
            endpointDispatcher.DispatchRuntime.CallbackClientRuntime.ClientMessageInspectors.Add(new BinaryWebSocketServiceActionExtracter());
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
