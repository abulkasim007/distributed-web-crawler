using Abstractions;
using Services;
using Events;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EventHandlers
{
    public class TextExtractedEventHandler : IEventHandler<TextExtractedEvent>
    {
        private readonly SearchIndex searchIndex;
        private readonly ILogger<TextExtractedEventHandler> logger;

        public TextExtractedEventHandler(SearchIndex searchIndex, ILogger<TextExtractedEventHandler> logger)
        {
            this.logger = logger;
            this.searchIndex = searchIndex;
        }

        public Task Handle(TextExtractedEvent @event)
        {
            this.searchIndex.Write(@event.Url.AbsoluteUri, @event.Text.Title, @event.Text.Content);

            logger.LogInformation($"Index for {@event.Url.AbsoluteUri} is done.");

            return Task.CompletedTask;
        }
    }
}
