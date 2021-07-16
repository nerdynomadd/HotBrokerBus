using System;
using System.Threading;
using System.Threading.Tasks;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Stan.Commands;
using HotBrokerBus.Stan.Events;
using HotBrokerBus.Stan.Extensions.Options.Event;
using Microsoft.Extensions.Hosting;
using STAN.Client;

namespace HotBrokerBus.Stan.HostedService
{
    public class StanBusHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public StanBusHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _startEventBus();
            
            _startCommandBus();

            return Task.CompletedTask;
        }

        private void _startEventBus()
        {
            var eventBusOptions = (StanEventBusOptions) _serviceProvider.GetService(typeof(StanEventBusOptions));

            if (eventBusOptions == null)
            {
                return;
            }
            
            var eventBusRegister = (IStanEventBusRegister) _serviceProvider.GetService(typeof(IStanEventBusRegister));

            if (eventBusRegister == null)
            {
                return;
            }
            
            foreach ((var eventSubscriptionName, var eventSubscriptionDefinition) in eventBusOptions.Subscriptions.Config.Events)
            {
                typeof(StanEventBusRegister)
                    .GetMethod(nameof(StanEventBusRegister.Subscribe),
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
            
            var commandBusRegister = (IStanCommandBusRegister) _serviceProvider.GetService(typeof(IStanCommandBusRegister));

            if (commandBusRegister == null)
            {
                return;
            }
            
            foreach ((var commandSubscriptionName, var commandSubscriptionDefinition) in commandBusOptions.Subscriptions.Config.Commands)
            {
                typeof(StanCommandBusRegister)
                    .GetMethod(nameof(StanCommandBusRegister.Subscribe), new Type[] {typeof(string)})?
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