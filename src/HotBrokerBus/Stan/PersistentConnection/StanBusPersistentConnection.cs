using System;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Stan.Extensions.Options.Modules;
using HotBrokerBus.Stan.Jobs;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using STAN.Client;

namespace HotBrokerBus.Stan.PersistentConnection
{
    public class StanBusPersistentConnection : IStanBusPersistentConnection
    {
        private readonly StanConnectionFactory _connectionFactory;

        private readonly StanModulesOptions _stanModulesOptions;

        private readonly IServiceProvider _serviceProvider;

        private IStanConnection _connection;

        public StanBusPersistentConnection(StanModulesOptions stanModulesOptions,
            IServiceProvider serviceProvider)
        {
            _connectionFactory = new StanConnectionFactory();

            _stanModulesOptions = stanModulesOptions;

            _serviceProvider = serviceProvider;
        }

        public void InitHooks()
        {
            _stanModulesOptions.ConnectionOptions.StanOptionsOptions.StanOptions.ConnectionLostEventHandler = async (obj, args) =>
            {
                using var scope = _serviceProvider.CreateScope();
                
                var schedulerFactory = _serviceProvider.GetService<ISchedulerFactory>();

                if (schedulerFactory == null) return;

                var scheduler = await schedulerFactory.GetScheduler();
                
                var integrationEventBus = _serviceProvider.GetService<IStanEventBusSubscriberClient>();

                var integrationCommandBus = _serviceProvider.GetService<IStanCommandBusSubscriberClient>();
                
                var stanPersistentConnection = _serviceProvider.GetService<IStanBusPersistentConnection>();

                var logger = _serviceProvider.GetService<ILogger<StanReconnectJob>>();
                
                logger.LogInformation("The connection with Stan message broker was lost. Reconnection process started...");

                var jobData = new JobDataMap
                {
                    {"integrationEventBus", integrationEventBus},
                    {"integrationCommandBus", integrationCommandBus},
                    {"stanPersistentConnection", stanPersistentConnection},
                    {"logger", logger}
                };

                var job = JobBuilder
                    .Create<StanReconnectJob>()
                    .UsingJobData(jobData)
                    .Build();

                var trigger = TriggerBuilder.Create()
                    .StartNow()
                    .WithIdentity("reconnect", "stan")
                    .WithSimpleSchedule(
                        x => x.WithIntervalInSeconds(5)
                            .RepeatForever())
                    .Build();

                await scheduler.ScheduleJob(job, trigger);

                await scheduler.Start();
            };
        }

        public IStanConnection CreateModel()
        {
            if (_connection?.NATSConnection is null || _connection.NATSConnection.IsClosed())
            {
                InitHooks();

                _connection = _connectionFactory.CreateConnection(
                    _stanModulesOptions.ConnectionOptions.ClusterName, 
                    _stanModulesOptions.ConnectionOptions.AppName, 
                    _stanModulesOptions.ConnectionOptions.StanOptionsOptions.StanOptions);
            }

            return _connection;
        }
    }
}