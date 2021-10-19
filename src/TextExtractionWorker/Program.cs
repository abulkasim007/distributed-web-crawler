using Abstractions;
using EventHandlers;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceBus;

namespace TextExtractionWorker
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
                    services.AddSingleton<IEventHandler<HtmlDownloadedTEEvent>, HtmlDownloadedTEEventHandler>();

                    services.AddServiceBus();
                });
    }
}
