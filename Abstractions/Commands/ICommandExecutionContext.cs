using System;
using Autofac;
using NATS.Client;
using STAN.Client;

namespace Bus.Abstractions.Commands
{
    public interface ICommandExecutionContext : IBusExecutionContext
    {
        public byte[] CommandData { get; }
        
        public Type CommandType { get; }
        
        public ICommand Command { get; set; }
        
        public Type CommandHandlerType { get; }
        
        public ICommandHandler CommandHandler { get; set; }
        
        public MsgHandlerEventArgs BusArguments { get; }
        
        public ILifetimeScope LifetimeScope { get; }
    }
}