using System;
using Autofac;
using NATS.Client;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Commands
{
    public interface ICommandExecutionContext : IBusExecutionContext
    {
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