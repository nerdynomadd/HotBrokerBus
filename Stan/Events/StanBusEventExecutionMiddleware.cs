using System;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Middleware;
using HotBrokerBus.Abstractions.Middleware.Events;

namespace HotBrokerBus.Stan.Events
{
    public class StanBusEventExecutionMiddleware : IStanBusEventExecutionMiddleware
    {
        
        public async Task Invoke(BusMiddlewareExecutionDelegate next, IEventExecutionContext context)
        {
            
            try
            {
                #nullable enable
                var eventResult = await ((Task<bool>?) context
                    .EventHandler?
                    .GetType()
                    .GetMethod("Handle")?
                    .Invoke(context.EventHandler, new object[] {context.Event}) ?? Task.FromResult(false));

                if (eventResult)
                {
                    context.BusArguments.Message.Ack();
                }

                await next(context);
                #nullable disable
            }
            catch (Exception e)
            {
                if (e.InnerException == null)
                {
                    throw;
                }
                
                throw e.InnerException;
            }

        }
    }
}