using Abstractions;
using DataStructures;

namespace Messages
{
    public class UrlExtractedEvent :IEvent
    {
        public UrlExtractedEvent()
        {

        }

        public UrlExtractedEvent(Url url, int depth)
        {
            Url = url;
            Depth = depth;
        }

        public Url Url { get; set; }

        public int Depth { get; set; }

        public int Port => 5558;
    }
}
