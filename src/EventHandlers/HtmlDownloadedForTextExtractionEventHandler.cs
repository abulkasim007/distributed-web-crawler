using Abstractions;
using Services;
using DataStructures;
using Events;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace EventHandlers
{
    public class HtmlDownloadedForTextExtractionEventHandler : IEventHandler<HtmlDownloadedForTextExtractionEvent>
    {
        private readonly IBus bus;
        private readonly ILogger<HtmlDownloadedForTextExtractionEventHandler> logger;

        public HtmlDownloadedForTextExtractionEventHandler(IBus bus, ILogger<HtmlDownloadedForTextExtractionEventHandler> logger)
        {
            this.bus = bus;
            this.logger = logger;
        }

        public Task Handle(HtmlDownloadedForTextExtractionEvent @event)
        {
            Text text = TextExtractor.Extract(@event.Html);

            logger.LogInformation($"{DateTime.Now} Text length: {text.Content?.Length} has been extracted. Page title: {text.Title}");

            TextExtractedEvent textExtractedEvent = new(@event.Url, text);

            bus.Publish(textExtractedEvent);

            return Task.CompletedTask;
        }
    }
}
