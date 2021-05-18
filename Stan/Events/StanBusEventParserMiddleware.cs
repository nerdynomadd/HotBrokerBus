using System;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Bus.Abstractions.Events;
using Bus.Abstractions.Stan.Events;
using Bus.Middleware;
using Newtonsoft.Json;

namespace Bus.Stan.Events
{
    public class StanBusEventParserMiddleware : IStanBusEventParserMiddleware
    {
        public async Task Invoke(BusMiddlewareExecutionDelegate next, IEventExecutionContext context)
        {
            var @event = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(context.EventData), context.EventType) as IEvent;

            if (@event == null) return;

            var eventHandler = context.LifetimeScope.ResolveOptional(context.EventHandlerType) as IEventHandler;

            if (eventHandler == null) return;

            context.Event = @event;
            
            context.EventHandler = eventHandler;

            await next(context);
        }
    }
}