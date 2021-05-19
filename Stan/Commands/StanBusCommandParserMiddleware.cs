using System.Text;
using System.Threading.Tasks;
using Autofac;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Middleware;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HotBrokerBus.Stan.Commands
{
    public class StanBusCommandParserMiddleware : IStanBusCommandParserMiddleware
    {
        public async Task Invoke(BusMiddlewareExecutionDelegate next, ICommandExecutionContext context)
        {
            ICommand command = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(context.CommandData), context.CommandType) as ICommand;

            if (command == null) return;
            
            var commandHandler = context.LifetimeScope.ResolveOptional(context.CommandHandlerType) as ICommandHandler;

            if (commandHandler == null) return;

            context.Command = command;
            
            context.CommandHandler = commandHandler;
            
            await next(context);
        }
    }
}