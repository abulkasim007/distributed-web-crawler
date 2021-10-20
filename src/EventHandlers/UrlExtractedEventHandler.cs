using Abstractions;
using Events;
using System.Threading.Tasks;
using System.Net.Http;
using System;
using Services;

namespace EventHandlers
{
    public class UrlExtractedEventHandler : IEventHandler<UrlExtractedEvent>
    {
        private const string HtmlMediaType = "text/html";

        private readonly IBus bus;
        private readonly IHttpClientFactory httpClientFactory;

        public UrlExtractedEventHandler(IBus bus, IHttpClientFactory httpClientFactory)
        {
            this.bus = bus;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task Handle(UrlExtractedEvent @event)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(HtmlClient.HttpHandlerName);

            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(@event.Url.AbsoluteUri, HttpCompletionOption.ResponseHeadersRead);

            bool validUrl = 
                   httpResponseMessage.IsSuccessStatusCode == true
                && httpResponseMessage.Content.Headers.ContentType.MediaType.Equals(HtmlMediaType, StringComparison.InvariantCultureIgnoreCase) == true;

            if (validUrl)
            {
                UrlExaminedEvent urlExaminedEvent = new UrlExaminedEvent(@event.Url);

                this.bus.Publish(urlExaminedEvent);
            }
        }
    }
}
