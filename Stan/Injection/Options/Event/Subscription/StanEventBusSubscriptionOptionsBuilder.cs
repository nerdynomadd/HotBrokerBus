using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Stan.Extensions.Options.Event.Event.Config;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Event;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Event.StanSubscriptionOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using STAN.Client;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Event
{
    public class StanEventBusSubscriptionOptionsBuilder
    {

        private List<StanEventEventSubscriptionParameters> _stanEventEventParameters;

        private readonly StanEventBusSubscriptionConfigOptionsBuilder _stanEventBusSubscriptionConfigOptionsBuilder;

        public StanEventBusSubscriptionOptionsBuilder(List<StanEventEventSubscriptionParameters> stanEventEventParameters, IServiceCollection serviceCollection)
        {

            _stanEventEventParameters = stanEventEventParameters;

            _stanEventBusSubscriptionConfigOptionsBuilder = new(_stanEventEventParameters, serviceCollection);

        }

        public StanEventBusSubscriptionOptionsBuilder Subscribe<T, T2>(string subscriptionName, string queueGroup, StanEventEventSubscriptionOptionsParameters subscriptionOptions = null)
            where T: IEvent
            where T2 : IEventHandler
        {
            _stanEventEventParameters.Add(new StanEventEventSubscriptionParameters
            {
                Type = typeof(T).Name,
                HandlerType = typeof(T2).Name,
                SubscriptionName = subscriptionName,
                QueueGroup =  queueGroup,
                SubscriptionOptions = subscriptionOptions
            });

            Bind<T, T2>();

            return this;
        }
        
        public StanEventBusSubscriptionOptionsBuilder Bind<T, T2>()
            where T: IEvent
            where T2 : IEventHandler
        {
            _stanEventBusSubscriptionConfigOptionsBuilder.Bind<T, T2>();

            return this;
        }

        public StanEventBusSubscriptionOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanEventEventParameters);

            return this;
        }

        internal StanEventBusSubscriptionOptions Build()
        {
            var configOptions = _stanEventBusSubscriptionConfigOptionsBuilder.Build();

            return new StanEventBusSubscriptionOptions(configOptions);
        }
        
    }
}