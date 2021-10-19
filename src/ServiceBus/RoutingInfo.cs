using System;
using System.Reflection;

namespace ServiceBus
{
    public class RoutingInfo
    {
        public RoutingInfo(int port, Type eventType, Type handlerType, MethodInfo handler)
        {
            Port = port;
            Handler = handler;
            EventType = eventType;
            HandlerType = handlerType;
        }

        public int Port { get; }

        public Type EventType { get; }

        public Type HandlerType { get; }

        public MethodInfo Handler { get; }
    }
}
