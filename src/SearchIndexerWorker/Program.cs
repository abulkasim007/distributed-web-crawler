using Abstractions;
using EventHandlers;
using Events;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ServiceBus;
using Services;
using System.Text.Json.Serialization;

namespace SearchIndexerWorker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                 .UseDefaultServiceProvider((serviceProviderOptions) =>
                 {
                     serviceProviderOptions.ValidateScopes = true;
                     serviceProviderOptions.ValidateOnBuild = true;
                 })
                 .ConfigureServices((hostContext, services) =>
                 {
                     services.AddHttpClient();

                     services.AddSingleton<SearchIndex>();

                     services.AddSingleton<IEventHandler<TextExtractedEvent>, TextExtractedEventHandler>();

                     services.AddRouting();

                     services.AddMvcCore((options) =>
                     {
                         options.SuppressAsyncSuffixInActionNames = false;
                     })
                     .AddJsonOptions(jsonOptions =>
                     {
                         jsonOptions.JsonSerializerOptions.PropertyNamingPolicy = null;
                         jsonOptions.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                     });

                     services.AddServiceBus();
                 })
                 .Configure(app =>
                 {
                     app.UseRouting();
                     app.UseEndpoints(e =>
                     {
                         e.MapControllerRoute("default", "{controller=Search}/{action=Get}/{id?}");
                     });
                 });
    }
}
