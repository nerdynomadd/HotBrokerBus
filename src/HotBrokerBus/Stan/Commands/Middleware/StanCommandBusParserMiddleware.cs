using System.Text;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Middleware;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace HotBrokerBus.Stan.Commands
{
    public class StanCommandBusParserMiddleware : IStanCommandBusParserMiddleware
    {
        public async Task Invoke(BusMiddlewareExecutionDelegate next, ICommandExecutionContext context)
        {
            using var serviceScope = context.ServiceProvider.CreateScope();

            ICommand command = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(context.CommandData), context.CommandType) as ICommand;

            if (command == null) return;
            
            var commandHandler = (ICommandHandler) context.ServiceProvider.GetService(context.CommandHandlerType);

            if (commandHandler == null) return;

            context.Command = command;
            
            context.CommandHandler = commandHandler;
            
            await next(context);
        }
    }
}