using Abstractions;
using Services;
using Events;
using System.Threading.Tasks;

namespace EventHandlers
{
    public class UrlFilteredEventHandler : IEventHandler<UrlFilteredEvent>
    {
        private readonly IBus bus;
        private readonly HtmlClient htmlClient;

        public UrlFilteredEventHandler(IBus bus, HtmlClient htmlClient)
        {
            this.bus = bus;
            this.htmlClient = htmlClient;
        }

        public async Task Handle(UrlFilteredEvent @event)
        {
            string html = await this.htmlClient.GetHtmlAsync(@event.Url);

            if (string.IsNullOrWhiteSpace(html))
            {
                return;
            }

            HtmlDownloadedTEEvent htmlDownloadedTEEvent = new(@event.Url, html);

            this.bus.Publish(htmlDownloadedTEEvent);

            HtmlDownloadedUEEvent htmlDownloadedUEEvent = new(@event.Url, html);

            this.bus.Publish(htmlDownloadedUEEvent);
        }
    }
}
