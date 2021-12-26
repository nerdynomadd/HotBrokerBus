using System;
using System.Text;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Middleware;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Commands.Middleware;
using NATS.Client;
using Newtonsoft.Json;

namespace HotBrokerBus.Stan.Commands.Middleware
{
    public class StanCommandBusExecutionMiddleware : IStanCommandBusExecutionMiddleware
    {
        public async Task Invoke(BusMiddlewareExecutionDelegate next, IStanCommandBusExecutionContext context)
        {
            try
            {
                #nullable enable
                var commandResult = await ((Task<object>?) context
                    .CommandHandler?
                    .GetType()
                    .GetMethod("Process")?
                    .Invoke(context.CommandHandler, new object[] { context.Command }) ?? Task.FromResult(new object()));

                var commandResultJson = JsonConvert.SerializeObject(commandResult);

                var replyMessage = new Msg
                {
                    Subject = context.BusArguments.Message.Reply,
                    Data = Encoding.UTF8.GetBytes(commandResultJson)
                };

                context.BusArguments.Message.Respond(Encoding.UTF8.GetBytes(commandResultJson));
                
                await next(context);
                #nullable disable
            } catch (Exception e)
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