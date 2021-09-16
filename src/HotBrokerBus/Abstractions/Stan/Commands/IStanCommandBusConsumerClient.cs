using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Commands
{
	public interface IStanCommandBusConsumerClient
	{
		public IStanConnection Connection { get; set; }
		
		public TIntegrationCommandResult Send<TIntegrationCommandResult>(string subject,
			ICommand<TIntegrationCommandResult> command)
			where TIntegrationCommandResult : ICommandResult;

		public Task SendAsync(string subject,
			ICommand command,
			int timeout = 5000);

		public Task<TIntegrationCommandResult> SendAsync<TIntegrationCommandResult>(string subject,
			ICommand<TIntegrationCommandResult> command,
			int timeout = 5000)
			where TIntegrationCommandResult : ICommandResult;
	}
}