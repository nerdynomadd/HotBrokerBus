using System.Threading.Tasks;
using Bus.Abstractions.Stan.Commands;
using Bus.Abstractions.Stan.Events;
using Bus.Stan.PersistentConnection;
using Microsoft.Extensions.Logging;
using Quartz;
using STAN.Client;

namespace Bus.Stan.Jobs
{
    public class ReconnectJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var logger = context.JobDetail.JobDataMap["logger"] as ILogger<ReconnectJob>;
            
            try
            {
                var stanIntegrationEventBus =
                    context.JobDetail.JobDataMap["integrationEventBus"] as IStanBusEventRegister;

                var stanIntegrationCommandBus =
                    context.JobDetail.JobDataMap["integrationCommandBus"] as IStanBusCommandRegister;

                var stanPersistentConnection =
                    context.JobDetail.JobDataMap["stanPersistentConnection"] as StanBusPersistentConnection;

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