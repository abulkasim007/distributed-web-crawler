using DataStructures;
using System.Net.Http;
using System.Threading.Tasks;

namespace Services
{
    public class HtmlClient
    {
        public const string HttpHandlerName = nameof(HtmlClient);

        private const string HtmlMediaType = "text/html";
        private readonly IHttpClientFactory httpClientFactory;

        public HtmlClient(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        public Task<string> GetHtmlAsync(Url url)
        {
            HttpClient httpClient = httpClientFactory.CreateClient(HttpHandlerName);

            return httpClient.GetStringAsync(url.AbsoluteUri);
        }
    }
}
