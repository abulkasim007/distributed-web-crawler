using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBus
{
    public class ServiceBusHostedService : IHostedService
    {
        private readonly IRoutingTable routingTable;
        private readonly IServiceProvider serviceProvider;
        private readonly ILogger<ServiceBusHostedService> logger;

        private readonly NetMQPoller netMQPoller = new();
        private readonly Dictionary<int, PullSocket> pullSockets = new();

        public ServiceBusHostedService(IRoutingTable routingTable, IServiceProvider serviceProvider, ILogger<ServiceBusHostedService> logger)
        {
            this.logger = logger;
            this.routingTable = routingTable;
            this.serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (int port in routingTable.Routes.Keys)
            {
                PullSocket pullSocket = new($">tcp://localhost:{port}");

                pullSockets.Add(port, pullSocket);

                netMQPoller.Add(pullSocket);

                pullSocket.ReceiveReady += Handle;
            }

            netMQPoller.RunAsync();

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            foreach (PullSocket pullSocket in pullSockets.Values)
            {
                netMQPoller.RemoveAndDispose(pullSocket);
            }

            netMQPoller.Dispose();

            return Task.CompletedTask;
        }

        private async void Handle(object sender, NetMQSocketEventArgs netMQSocketEventArgs)
        {
            try
            {
                string messageJson = netMQSocketEventArgs.Socket.ReceiveFrameString();

                Message message = JsonSerializer.Deserialize<Message>(messageJson);

                RoutingInfo routingInfo = routingTable.Routes[message.Port];

                logger.LogInformation($"Handling event: {routingInfo.EventType}");

                object @event = JsonSerializer.Deserialize(message.Content, routingInfo.EventType);

                object eventHandler = serviceProvider.GetService(routingInfo.HandlerType);

                await (Task)routingInfo.Handler.Invoke(eventHandler, new[] { @event });

                logger.LogInformation($"Event: {routingInfo.EventType} is handled.");
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
            }
        }
    }
}
