using System.Collections.Generic;

namespace ServiceBus
{
    public interface IRoutingTable
    {
        IDictionary<int, RoutingInfo> Routes { get; }
    }
}
