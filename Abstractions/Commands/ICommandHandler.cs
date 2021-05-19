using System.Threading.Tasks;

namespace HotBrokerBus.Abstractions.Commands
{
    public interface ICommandHandler
    {
    }
    
    public interface ICommandHandler<in TIntegrationCommand, out TIntegrationCommandResult> : ICommandHandler
        where TIntegrationCommand : ICommand<TIntegrationCommandResult>
        where TIntegrationCommandResult : ICommandResult
    {
        Task<object> Process(TIntegrationCommand command);
    }

    public interface ICommandHandler<in TIntegrationCommand> : ICommandHandler
        where TIntegrationCommand : ICommand
    {
        Task Process(TIntegrationCommand command);
    }
}