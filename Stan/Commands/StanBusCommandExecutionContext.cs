using System;
using Autofac;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Middleware;
using NATS.Client;
using STAN.Client;

namespace HotBrokerBus.Stan.Commands
{
    public class StanBusCommandExecutionContext : ICommandExecutionContext
    {
        public StanBusCommandExecutionContext(BusMiddlewareComponent middlewareComponent,
            string commandTopic,
            byte[] commandData,
            Type commandType,
            ICommand command,
            Type commandHandlerType,
            ICommandHandler commandHandler,
            MsgHandlerEventArgs busArguments,
            ILifetimeScope lifetimeScope)
        {
            MiddlewareComponent = middlewareComponent;

            CommandTopic = commandTopic;

            CommandData = commandData;

            CommandType = commandType;

            Command = command;

            CommandHandlerType = commandHandlerType;

            CommandHandler = commandHandler;

            BusArguments = busArguments;

            LifetimeScope = lifetimeScope;
        }
        
        public BusMiddlewareComponent MiddlewareComponent { get; }
        
        public string CommandTopic { get; }
        
        public byte[] CommandData { get; }
        
        public Type CommandType { get; }
        
        public ICommand Command { get; set; }
        
        public Type CommandHandlerType { get; }
        
        public ICommandHandler CommandHandler { get; set; }
        
        public MsgHandlerEventArgs BusArguments { get; }
        
        public ILifetimeScope LifetimeScope { get; }
    }
}