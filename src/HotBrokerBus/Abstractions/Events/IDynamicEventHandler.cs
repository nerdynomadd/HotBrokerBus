using System.Threading.Tasks;

namespace HotBrokerBus.Abstractions.Events
{
    public interface IDynamicEventHandler
    {
        Task Handle(dynamic data);
    }
}