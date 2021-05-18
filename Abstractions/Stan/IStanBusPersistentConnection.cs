using STAN.Client;

namespace Bus.Abstractions.Stan
{
    public interface IStanBusPersistentConnection : IBusPersistentConnection<IStanConnection>
    {
    }
}