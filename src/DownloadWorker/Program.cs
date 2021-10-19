using Abstractions;
using EventHandlers;
using Messages;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Bulkhead;
using ServiceBus;
using Services;
using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace DownloadWorker
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
                    int maxParallelism = 100;

                    AsyncBulkheadPolicy<HttpResponseMessage> throttler = Policy.BulkheadAsync<HttpResponseMessage>(maxParallelism, int.MaxValue);

                    services
                    .AddHttpClient(HtmlClient.HttpHandlerName, client =>
                    {
                        client.DefaultRequestVersion = new Version(2, 0);
                        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36");
                    }).AddPolicyHandler(throttler);

                    services.AddSingleton<HtmlClient>();

                    services.AddSingleton<IEventHandler<UrlFilteredEvent>, UrlFilteredEventHandler>();

                    services.AddServiceBus();
                });
    }
}
