using Autofac;
using Bus.Abstractions.Stan;
using Bus.Stan.Commands;
using Bus.Stan.Events;
using Bus.Stan.Jobs;
using Microsoft.Extensions.Logging;
using Quartz;
using STAN.Client;

namespace Bus.Stan.PersistentConnection
{
    public class StanBusPersistentConnection : IStanBusPersistentConnection
    {
        private readonly StanConnectionFactory _connectionFactory;

        private readonly string _clusterName;

        private readonly string _appName;

        private readonly StanOptions _stanOptions;

        private readonly ILifetimeScope _lifetimeScope;

        private IStanConnection _connection;

        public StanBusPersistentConnection(string clusterName,
            string appName,
            StanOptions stanOptions,
            ILifetimeScope lifetimeScope)
        {
            _connectionFactory = new StanConnectionFactory();

            _clusterName = clusterName;

            _appName = appName;

            _stanOptions = stanOptions;

            _lifetimeScope = lifetimeScope;
        }

        public void InitHooks()
        {
            _stanOptions.ConnectionLostEventHandler = async (obj, args) =>
            {
                using (var scope = _lifetimeScope.BeginLifetimeScope())
                {
                    var scheduler = scope.Resolve<IScheduler>();

                    var integrationEventBus = scope.Resolve<StanBusEventRegister>();

                    var integrationCommandBus = scope.Resolve<StanBusCommandRegister>();

                    var logger = scope.Resolve<ILogger<ReconnectJob>>();

                    var jobData = new JobDataMap
                    {
                        {"integrationEventBus", integrationEventBus},
                        {"integrationCommandBus", integrationCommandBus},
                        {"stanPersistentConnection", this},
                        {"logger", logger}
                    };

                    var job = JobBuilder
                        .Create<ReconnectJob>()
                        .UsingJobData(jobData)
                        .Build();

                    var trigger = TriggerBuilder.Create()
                        .StartNow()
                        .WithIdentity("reconnect", "stan")
                        .WithSimpleSchedule(
                            x => x.WithIntervalInMinutes(1)
                                .RepeatForever())
                        .Build();

                    await scheduler.ScheduleJob(job, trigger);

                    await scheduler.Start();
                }
            };
        }

        public IStanConnection CreateModel()
        {
            if (_connection == null || _connection.NATSConnection == null || _connection.NATSConnection.IsClosed())
            {
                InitHooks();

                _connection = _connectionFactory.CreateConnection(_clusterName, _appName, _stanOptions);
            }

            return _connection;
        }
    }
}