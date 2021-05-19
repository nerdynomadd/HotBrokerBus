using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Commands
{
    public interface IStanBusCommandRegister
    {
        public IStanConnection Connection { get; set; }

        internal void Resume();

        internal void SetConnection(IStanConnection connection);

        public TIntegrationCommandResult Send<TIntegrationCommandResult>(string subject,
            ICommand<TIntegrationCommandResult> command)
            where TIntegrationCommandResult : ICommandResult;

        public TIntegrationCommandResult Send<TIntegrationCommandResult>(string subject,
            string commandName,
            ICommand<TIntegrationCommandResult> command)
            where TIntegrationCommandResult : ICommandResult;

        public Task SendAsync(string subject,
            ICommand command,
            int timeout = 5000);

        public Task SendAsync(string subject,
            string commandName,
            ICommand command,
            int timeout = 5000);
        
        public Task<TIntegrationCommandResult> SendAsync<TIntegrationCommandResult>(string subject,
            ICommand<TIntegrationCommandResult> command,
            int timeout = 5000)
            where TIntegrationCommandResult : ICommandResult;

        public Task<TIntegrationCommandResult> SendAsync<TIntegrationCommandResult>(string subject,
            string commandName,
            ICommand<TIntegrationCommandResult> command,
            int timeout = 5000)
            where TIntegrationCommandResult : ICommandResult;
        
        public void Subscribe<T, TH>(string subject)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>;

        public void Subscribe<T, TH>(string subject,
            string commandName)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>;

        void SubscribeDynamic<TH>(string subject,
            string commandName)
            where TH : IDynamicIntegrationCommandHandler;

        public void Unsubscribe<T, TH>(string subject)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>;

        public void UnsubscribeDynamic<TH>(string subject,
            string commandName)
            where TH : IDynamicEventHandler;
    }
}