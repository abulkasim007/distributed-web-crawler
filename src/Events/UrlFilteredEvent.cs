using Abstractions;
using DataStructures;

namespace Events
{
    public class UrlFilteredEvent : IEvent
    {
        public UrlFilteredEvent()
        {

        }

        public Url Url { get; set; }

        public UrlFilteredEvent(Url url)
        {
            Url = url;
        }

        public int Port => 5559;
    }
}
