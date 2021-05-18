using System;
using System.Text;
using System.Threading.Tasks;
using Bus.Abstractions.Commands;
using Bus.Abstractions.Middleware.Commands;
using Bus.Abstractions.Stan.Commands;
using Bus.Middleware;
using NATS.Client;
using Newtonsoft.Json;

namespace Bus.Stan.Commands
{
    public class StanBusCommandExecutionMiddleware : IStanBusCommandExecutionMiddleware
    {
        public async Task Invoke(BusMiddlewareExecutionDelegate next, ICommandExecutionContext context)
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

                var replyMessage = new Msg();

                replyMessage.Subject = context.BusArguments.Message.Reply;
                replyMessage.Data = Encoding.UTF8.GetBytes(commandResultJson);

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