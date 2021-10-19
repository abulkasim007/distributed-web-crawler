using Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ServiceBus
{
    public class RoutingTable : IRoutingTable
    {
        private const string GenericMessageHandlerTypeName = "IEventHandler`1";

        public RoutingTable(IServiceCollection serviceCollection)
        {
            BuildRoutingTable(serviceCollection);
        }

        public IDictionary<int, RoutingInfo> Routes { get; } = new Dictionary<int, RoutingInfo>();

        private void BuildRoutingTable(IServiceCollection serviceCollection)
        {
            foreach (var serviceDescriptor in serviceCollection)
            {
                if (serviceDescriptor.ImplementationType == null)
                {
                    continue;
                }

                IEnumerable<MethodInfo> eventHandlerMethods = GetMessageHandlerMethods(serviceDescriptor.ImplementationType);

                foreach (MethodInfo eventHandlerMethod in eventHandlerMethods)
                {
                    Type eventType = eventHandlerMethod.GetParameters().First().ParameterType;

                    int port = (Activator.CreateInstance(eventType) as IEvent).Port;

                    var routingInfo = new RoutingInfo(port, eventType, serviceDescriptor.ServiceType, eventHandlerMethod);

                    Routes.Add(port, routingInfo);
                }
            }
        }

        private static IEnumerable<MethodInfo> GetMessageHandlerMethods(Type implementorType)
        {
            return implementorType
                .GetTypeInfo()
                .ImplementedInterfaces
                .Where(interfaceType => interfaceType.Name.Equals(GenericMessageHandlerTypeName))
                .Select(interfaceType => implementorType.GetInterfaceMap(interfaceType))
                .SelectMany(map => Enumerable.Range(0, map.TargetMethods.Length)
                .Select(n => map.TargetMethods[n]));
        }
    }
}
