using Abstractions;
using DataStructures;

namespace Messages
{
    public class TextExtractedEvent :IEvent
    {
        public TextExtractedEvent()
        {

        }

        public TextExtractedEvent(Url url, Text text)
        {
            Url = url;
            Text = text;
        }

        public Url Url { get; set; }

        public Text Text { get; set; }

        public int Port => 5556;
    }
}
