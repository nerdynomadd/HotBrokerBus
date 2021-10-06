using System;

namespace HotBrokerBus.Abstractions.Commands
{
    public interface ICommand
    {
        Guid Id { get; }

        DateTime CreationDate { get; }
    }

    public interface ICommand<out TIntegrationCommandResult> : ICommand
        where TIntegrationCommandResult : ICommandResult
    {
    }
}