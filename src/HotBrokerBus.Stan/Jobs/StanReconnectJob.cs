using System;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Events;
using Microsoft.Extensions.Logging;
using NATS.Client;
using Quartz;
using STAN.Client;

namespace HotBrokerBus.Stan.Jobs
{
    public class StanReconnectJob : IStanReconnectJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var logger = context.JobDetail.JobDataMap["logger"] as ILogger<StanReconnectJob>;

            try
            {
                var stanIntegrationEventBus =
                    context.JobDetail.JobDataMap["integrationEventBus"] as IStanEventBusSubscriberClient;

                var stanIntegrationCommandBus =
                    context.JobDetail.JobDataMap["integrationCommandBus"] as IStanCommandBusSubscriberClient;

                var stanPersistentConnection =
                    context.JobDetail.JobDataMap["stanPersistentConnection"] as IStanBusPersistentConnection;

                var stanConnection = stanPersistentConnection?.CreateModel();

                stanIntegrationEventBus?.SetConnection(stanConnection);

                stanIntegrationEventBus?.Resume();

                stanIntegrationCommandBus?.SetConnection(stanConnection);

                stanIntegrationCommandBus?.Resume();

                await context.Scheduler.Clear();

                logger.LogInformation("The connection with Stan message broker was successfully resolved");
            }
            catch (Exception e) when (e is NATSConnectionException || e is StanConnectionException)
            {
                logger.LogError(e, "An error happened while trying to resume connection with Stan message broker");
            }
        }
    }
}