using Abstractions;
using DataStructures;
using Messages;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace UrlExtractionWorker
{
    internal class SeedHostedService : IHostedService
    {
        private readonly IBus bus;
        private readonly ILogger logger;
        private readonly IHostApplicationLifetime appLifetime;

        public SeedHostedService(
            IBus bus,
            ILogger<SeedHostedService> logger,
            IHostApplicationLifetime appLifetime)
        {
            this.bus = bus;
            this.logger = logger;
            this.appLifetime = appLifetime;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            appLifetime.ApplicationStarted.Register(OnStarted);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void OnStarted()
        {
            logger.LogInformation("Starting system with seed url.");

            UrlExtractedEvent urlExtractedEvent = new UrlExtractedEvent(new Url(0, "https://www.wikipedia.org/"), 0);

            bus.Publish(urlExtractedEvent);
        }
    }
}
