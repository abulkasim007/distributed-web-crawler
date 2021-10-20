using Abstractions;
using EventHandlers;
using Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceBus;

namespace UrlExtractionWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .UseDefaultServiceProvider((serviceProviderOptions) =>
                 {
                     serviceProviderOptions.ValidateScopes = true;
                     serviceProviderOptions.ValidateOnBuild = true;
                 })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IEventHandler<HtmlDownloadedForUrlExtractionEvent>, HtmlDownloadedForUrlExtractionEventHandler>();

                    services.AddServiceBus();

                    services.AddHostedService<SeedHostedService>();
                });
    }
}
