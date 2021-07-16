using System;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Middleware;
using NATS.Client;

namespace HotBrokerBus.Stan.Commands
{
    public class StanCommandBusExecutionContext : ICommandExecutionContext
    {
        public StanCommandBusExecutionContext(BusMiddlewareComponent middlewareComponent,
            string commandTopic,
            byte[] commandData,
            Type commandType,
            ICommand command,
            Type commandHandlerType,
            ICommandHandler commandHandler,
            MsgHandlerEventArgs busArguments,
            IServiceProvider serviceProvider)
        {
            MiddlewareComponent = middlewareComponent;

            CommandTopic = commandTopic;

            CommandData = commandData;

            CommandType = commandType;

            Command = command;

            CommandHandlerType = commandHandlerType;

            CommandHandler = commandHandler;

            BusArguments = busArguments;

            ServiceProvider = serviceProvider;
        }
        
        public BusMiddlewareComponent MiddlewareComponent { get; }
        
        public string CommandTopic { get; }
        
        public byte[] CommandData { get; }
        
        public Type CommandType { get; }
        
        public ICommand Command { get; set; }
        
        public Type CommandHandlerType { get; }
        
        public ICommandHandler CommandHandler { get; set; }
        
        public MsgHandlerEventArgs BusArguments { get; }
        
        public IServiceProvider ServiceProvider { get; }
    }
}