using Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ServiceBus
{
    public static class ServiceBusExtensions
    {
        public static IServiceCollection AddServiceBus(this IServiceCollection services)
        {
            services.AddSingleton<IBus, Bus>();

            var routingTable = new RoutingTable(services);

            services.AddSingleton<IRoutingTable>(routingTable);

            services.AddHostedService<ServiceBusHostedService>();

            return services;
        }
    }
}
