using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Stan.PersistentConnection;
using Microsoft.Extensions.Logging;
using Quartz;
using STAN.Client;

namespace HotBrokerBus.Stan.Jobs
{
    public class ReconnectJob : IReconnectJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var logger = context.JobDetail.JobDataMap["logger"] as ILogger<ReconnectJob>;
            
            try
            {
                var stanIntegrationEventBus =
                    context.JobDetail.JobDataMap["integrationEventBus"] as IStanEventBusRegister;

                var stanIntegrationCommandBus =
                    context.JobDetail.JobDataMap["integrationCommandBus"] as IStanCommandBusRegister;

                var stanPersistentConnection =
                    context.JobDetail.JobDataMap["stanPersistentConnection"] as IStanBusPersistentConnection;

                stanIntegrationEventBus?.SetConnection(stanPersistentConnection?.CreateModel());

                stanIntegrationEventBus?.Resume();

                stanIntegrationCommandBus?.SetConnection(stanPersistentConnection?.CreateModel());

                stanIntegrationCommandBus?.Resume();

                await context.Scheduler.Clear();
                
                logger.LogInformation("The connection with Stan message broker was successfully resolved");
            }
            catch (StanException e)
            {
                logger.LogError("An error happened while trying to resume connection with Stan message broker", e);
            }
        }
    }
}