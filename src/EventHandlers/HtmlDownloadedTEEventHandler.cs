using Abstractions;
using Services;
using DataStructures;
using Messages;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

namespace EventHandlers
{
    public class HtmlDownloadedTEEventHandler : IEventHandler<HtmlDownloadedTEEvent>
    {
        private readonly IBus bus;
        private readonly ILogger<HtmlDownloadedTEEventHandler> logger;

        public HtmlDownloadedTEEventHandler(IBus bus, ILogger<HtmlDownloadedTEEventHandler> logger)
        {
            this.bus = bus;
            this.logger = logger;
        }

        public Task Handle(HtmlDownloadedTEEvent @event)
        {
            Text text = TextExtractor.Extract(@event.Html);

            logger.LogInformation($"{DateTime.Now} Text length: {text.Content?.Length} has been extracted. Page title: {text.Title}");

            TextExtractedEvent textExtractedEvent = new(@event.Url, text);

            bus.Publish(textExtractedEvent);

            return Task.CompletedTask;
        }
    }
}
