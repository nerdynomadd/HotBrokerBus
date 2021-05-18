using System.Threading.Tasks;

namespace Bus.Abstractions.Events
{
    public interface IDynamicEventHandler
    {
        Task Handle(dynamic data);
    }
}