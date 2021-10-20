using Abstractions;
using DataStructures;

namespace Events
{
    public class UrlExaminedEvent : IEvent
    {
        public UrlExaminedEvent()
        {
        }

        public UrlExaminedEvent(Url url)
        {
            Url = url;
        }

        public Url Url { get; set; }

        public int Port => 5557;
    }
}
