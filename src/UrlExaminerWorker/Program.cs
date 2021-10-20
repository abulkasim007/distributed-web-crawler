using Abstractions;
using EventHandlers;
using Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Bulkhead;
using ServiceBus;
using Services;
using System;
using System.Net.Http;

namespace UrlExaminerWorker
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
                    }).AddPolicyHandler(throttler);


                    services.AddSingleton<IEventHandler<UrlExtractedEvent>, UrlExtractedEventHandler>();

                    services.AddServiceBus();
                });
    }
}
