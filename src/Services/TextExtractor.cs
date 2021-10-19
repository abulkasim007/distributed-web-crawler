using DataStructures;
using HtmlAgilityPack;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Services
{
    public class TextExtractor
    {
        private const string TextNodeSelector = "//text()";
        private const string TitleNodeSelector = "//title";
        private const string RegExPattern = @"[\u000A\u000B\u000C\u000D\u2028\u2029\u0085\u0009]+";

        public static Text Extract(string html)
        {
            string pageTitle = null;

            HtmlDocument htmlDocument = new() { OptionFixNestedTags = true };

            htmlDocument.LoadHtml(html);

            HtmlNode titleNode = htmlDocument.DocumentNode.SelectSingleNode(TitleNodeSelector);

            if (titleNode != null)
            {
                pageTitle = titleNode.InnerText;
            }

            htmlDocument.DocumentNode.Descendants().Where(Filter).ToList().ForEach(n => n.Remove());

            HtmlNodeCollection htmlNodeCollection = htmlDocument.DocumentNode.SelectNodes(TextNodeSelector);

            if (htmlNodeCollection == null)
            {
                return new Text(pageTitle, null);
            }

            StringBuilder stringBuilder = new();

            foreach (HtmlTextNode htmlTextNode in htmlNodeCollection)
            {
                stringBuilder.AppendLine(htmlTextNode.Text);
            }

            string sanitizedText = Regex.Replace(stringBuilder.ToString(), RegExPattern, string.Empty);

            return new Text(pageTitle, sanitizedText);
        }

        private static bool Filter(HtmlNode htmlNode)
        {
            const string styleNodeTypeName = "style";
            const string scriptNodeTypeName = "script";
            const string commentNodeTypeName = "#comment";

            return htmlNode.Name == scriptNodeTypeName || htmlNode.Name == styleNodeTypeName || htmlNode.Name == commentNodeTypeName;
        }
    }
}
