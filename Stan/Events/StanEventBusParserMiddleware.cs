using System.Text;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Middleware;
using Newtonsoft.Json;

namespace HotBrokerBus.Stan.Events
{
    public class StanEventBusParserMiddleware : IStanEventBusParserMiddleware
    {
        public async Task Invoke(BusMiddlewareExecutionDelegate next, IEventExecutionContext context)
        {
            var @event = (IEvent) JsonConvert.DeserializeObject(Encoding.UTF8.GetString(context.EventData), context.EventType);

            if (@event == null) return;

            var eventHandler = (IEventHandler) context.ServiceProvider.GetService(context.EventHandlerType);

            if (eventHandler == null) return;

            context.Event = @event;
            
            context.EventHandler = eventHandler;

            await next(context);
        }
    }
}