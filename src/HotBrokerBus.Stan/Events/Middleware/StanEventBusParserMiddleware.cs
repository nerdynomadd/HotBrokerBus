﻿using System.Text;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Middleware;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Abstractions.Stan.Events.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace HotBrokerBus.Stan.Events.Middleware
{
    public class StanEventBusParserMiddleware : IStanEventBusParserMiddleware
    {
        public async Task Invoke(BusMiddlewareExecutionDelegate next, IStanEventBusExecutionContext context)
        {
            using var serviceScope = context.ServiceProvider.CreateScope();
        
            var @event = (IEvent) JsonConvert.DeserializeObject(Encoding.UTF8.GetString(context.EventData), context.EventType);

            if (@event == null) return;

            var eventHandler = (IEventHandler) serviceScope.ServiceProvider.GetService(context.EventHandlerType);

            if (eventHandler == null) return;

            context.Event = @event;
            
            context.EventHandler = eventHandler;

            await next(context);
        }
    }
}