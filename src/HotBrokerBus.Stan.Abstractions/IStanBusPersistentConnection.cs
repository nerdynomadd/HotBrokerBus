using System.Threading.Tasks;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan
{
    public interface IStanBusPersistentConnection : IBusPersistentConnection<IStanConnection>
    {
        public Task SetupReconnect();
    }
}