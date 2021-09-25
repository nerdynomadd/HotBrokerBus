using System;
using System.Threading;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Stan.Commands;
using HotBrokerBus.Stan.Events;
using HotBrokerBus.Stan.Extensions.Configuration.Modules;
using HotBrokerBus.Stan.Extensions.Options.Event;
using HotBrokerBus.Stan.Extensions.Options.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NATS.Client;
using STAN.Client;

namespace HotBrokerBus.Stan.HostedService
{
    public class StanBusHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly StanModulesOptions _stanModulesOptions;

        private readonly ILogger<StanBusHostedService> _logger;

        public StanBusHostedService(IServiceProvider serviceProvider,
            StanModulesOptions stanModulesOptions,
            ILogger<StanBusHostedService> logger)
        {
            _serviceProvider = serviceProvider;

            _stanModulesOptions = stanModulesOptions;

            _logger = logger;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            if (_stanModulesOptions.HostedServiceOptions.Active)
            {
                void StartBuses()
                {
                    _startEventBus();

                    _startCommandBus();
                }

                if (_stanModulesOptions.HostedServiceOptions.ThrowsStartingException)
                {
                    StartBuses();
                } else
                {
                    try
                    {
                        StartBuses();
                    }
                    catch (NATSConnectionException connectionException)
                    {
                        _logger.LogError($"An error happened while trying to connect to a NATS cluster: {connectionException}");
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError($"An error happened while trying to start Stan buses: {exception}");
                    }
                }
            }

            return Task.CompletedTask;
        }

        private void _startEventBus()
        {
            var eventBusOptions = (StanEventBusOptions) _serviceProvider.GetService(typeof(StanEventBusOptions));

            if (eventBusOptions is null)
            {
                return;
            }
            
            var eventBusRegister = (IStanEventBusSubscriberClient) _serviceProvider.GetService(typeof(IStanEventBusSubscriberClient));

            if (eventBusRegister is null)
            {
                return;
            }
            
            foreach ((var eventSubscriptionName, var eventSubscriptionDefinition) in eventBusOptions.Subscriptions.Config.Events)
            {
                typeof(StanEventBusSubscriberClient)
                    .GetMethod(nameof(StanEventBusSubscriberClient.Subscribe),
                        new Type[] {typeof(string), typeof(string), typeof(StanSubscriptionOptions)})?
                    .MakeGenericMethod(eventSubscriptionDefinition.Item1, eventSubscriptionDefinition.Item2)
                    .Invoke(eventBusRegister, new object?[] { eventSubscriptionName, eventSubscriptionDefinition.Item3, eventSubscriptionDefinition.Item4 });
            }
        }
        
        private void _startCommandBus()
        {
            var commandBusOptions = (StanCommandBusOptions) _serviceProvider.GetService(typeof(StanCommandBusOptions));

            if (commandBusOptions == null)
            {
                return;
            }
            
            var commandBusRegister = (IStanCommandBusSubscriberClient) _serviceProvider.GetService(typeof(IStanCommandBusSubscriberClient));

            if (commandBusRegister == null)
            {
                return;
            }
            
            foreach ((var commandSubscriptionName, var commandSubscriptionDefinition) in commandBusOptions.Subscriptions.Config.Commands)
            {
                typeof(StanCommandBusSubscriberClient)
                    .GetMethod(nameof(StanCommandBusSubscriberClient.Subscribe), new Type[] {typeof(string)})?
                    .MakeGenericMethod(commandSubscriptionDefinition.Item1, commandSubscriptionDefinition.Item2)
                    .Invoke(commandBusRegister, new object?[]
                    {
                        commandSubscriptionName
                    });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}