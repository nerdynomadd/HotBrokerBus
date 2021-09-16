using System;
using HotBrokerBus.Abstractions.Commands;

namespace HotBrokerBus.Commands
{
    public class Command<TResult> : ICommand<TResult>
        where TResult : ICommandResult
    {
        public Guid Id { get; } = new ();
        
        public DateTime CreationDate { get; } = DateTime.Now;
    }
}