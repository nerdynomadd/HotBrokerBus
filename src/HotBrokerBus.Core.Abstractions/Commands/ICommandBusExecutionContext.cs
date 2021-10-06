using System;

namespace HotBrokerBus.Abstractions.Commands
{
    public interface ICommandBusExecutionContext : IBusExecutionContext
    {
        public string CommandTopic { get; }
        
        public byte[] CommandData { get; }
        
        public Type CommandType { get; }
        
        public ICommand Command { get; set; }
        
        public Type CommandHandlerType { get; }
        
        public ICommandHandler CommandHandler { get; set; }
        
        public IServiceProvider ServiceProvider { get; }
    }
}