using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Commands;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Commands
{
    public interface IStanCommandBusSubscriberClient
    {
        public IStanConnection Connection { get; set; }

        void Resume();

        void SetConnection(IStanConnection connection);

        public void Subscribe<T, TH>(string subject)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>;
        
        public void Unsubscribe<T, TH>(string subject)
            where T : ICommand<ICommandResult>
            where TH : ICommandHandler<T, ICommandResult>;
    }
}