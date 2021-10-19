using DataStructures;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Services
{
    public class UrlExtractor
    {
        private const string Href = "href";

        public static IEnumerable<Url> Extract(Url parentUrl, string html)
        {
            Uri baseUri = new(parentUrl.AbsoluteUri);

            HtmlDocument htmlDocument = new() { OptionFixNestedTags = true };

            htmlDocument.LoadHtml(html);

            IEnumerable<string> hrefs = htmlDocument.DocumentNode.Descendants("a")
                               .Select(a => a.GetAttributeValue(Href, null))
                               .Where(u => !string.IsNullOrEmpty(u));

            foreach (string href in hrefs)
            {
                bool created = Uri.TryCreate(baseUri, href, out Uri childUri);

                if (created == true && (childUri.Scheme == Uri.UriSchemeHttp || childUri.Scheme == Uri.UriSchemeHttps))
                {
                    yield return new Url(parentUrl.Depth + 1, childUri.AbsoluteUri);
                }
            }
        }
    }
}
