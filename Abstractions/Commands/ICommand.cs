using System;

namespace Bus.Abstractions.Commands
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