using System.Text;
using System.Threading.Tasks;
using Autofac;
using Bus.Abstractions.Commands;
using Bus.Abstractions.Stan.Commands;
using Bus.Middleware;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bus.Stan.Commands
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