using System.Threading.Tasks;

namespace Abstractions
{
    public interface IEventHandler<T> where T : IEvent
    {
        Task Handle(T @event);
    }
}
