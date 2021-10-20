using Abstractions;
using Services;
using Events;
using System.Threading.Tasks;

namespace EventHandlers
{
    public class UrlExaminedEventHandler : IEventHandler<UrlExaminedEvent>
    {
        private readonly IBus bus;

        public UrlExaminedEventHandler(IBus bus)
        {
            this.bus = bus;
        }

        public Task Handle(UrlExaminedEvent @event)
        {
            bool knownUrl = UrlFilter.Filter(@event.Url);

            if (knownUrl == false)
            {
                UrlFilteredEvent urlFilteredEvent = new UrlFilteredEvent(@event.Url);

                this.bus.Publish(urlFilteredEvent);
            }

            return Task.CompletedTask;
        }
    }
}
