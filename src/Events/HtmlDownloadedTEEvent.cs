using Abstractions;
using DataStructures;

namespace Events
{
    public class HtmlDownloadedTEEvent : IEvent
    {
        public HtmlDownloadedTEEvent()
        {

        }

        public HtmlDownloadedTEEvent(Url url, string html)
        {
            Url = url;
            Html = html;
        }

        public Url Url { get; set; }

        public string Html { get; set; }

        public int Port => 5554;
    }
}
