using Abstractions;
using Services;
using DataStructures;
using Messages;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EventHandlers
{
    public class HtmlDownloadedUEEventHandler : IEventHandler<HtmlDownloadedUEEvent>
    {
        private readonly IBus bus;
        private readonly ILogger<HtmlDownloadedUEEventHandler> logger;

        public HtmlDownloadedUEEventHandler(IBus bus, ILogger<HtmlDownloadedUEEventHandler> logger)
        {
            this.bus = bus;
            this.logger = logger;
        }

        public Task Handle(HtmlDownloadedUEEvent @event)
        {
            IEnumerable<Url> urls = UrlExtractor.Extract(@event.Url, @event.Html);

            int counter = 0;

            foreach (Url url in urls)
            {
                UrlExtractedEvent urlExtractedEvent = new(url, @event.Url.Depth);

                bus.Publish(urlExtractedEvent);

                counter++;
            }

            logger.LogInformation($"Total {counter} url(s) extracted");

            return Task.CompletedTask;
        }
    }
}
