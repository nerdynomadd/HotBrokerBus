using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Stan.Extensions.Options.Event.Event.Config;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Event;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Event.StanSubscriptionOptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using STAN.Client;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Event
{
    public class StanCommandBusSubscriptionOptionsBuilder
    {

        private List<StanCommandCommandSubscriptionParameters> _stanCommandCommandParameters;

        private readonly StanCommandBusSubscriptionConfigOptionsBuilder _stanCommandBusSubscriptionConfigOptionsBuilder;

        public StanCommandBusSubscriptionOptionsBuilder(List<StanCommandCommandSubscriptionParameters> stanCommandCommandParameters, IServiceCollection serviceCollection)
        {

            _stanCommandCommandParameters = stanCommandCommandParameters;

            _stanCommandBusSubscriptionConfigOptionsBuilder = new(_stanCommandCommandParameters, serviceCollection);

        }

        public StanCommandBusSubscriptionOptionsBuilder Subscribe<T, T2>(string subscriptionName)
            where T: ICommand
            where T2 : ICommandHandler
        {
            _stanCommandCommandParameters.Add(new StanCommandCommandSubscriptionParameters
            {
                Type = typeof(T).Name,
                HandlerType = typeof(T2).Name,
                SubscriptionName = subscriptionName
            });

            Bind<T, T2>();

            return this;
        }
        
        public StanCommandBusSubscriptionOptionsBuilder Bind<T, T2>()
            where T: ICommand
            where T2 : ICommandHandler
        {
            _stanCommandBusSubscriptionConfigOptionsBuilder.Bind<T, T2>();

            return this;
        }

        public StanCommandBusSubscriptionOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanCommandCommandParameters);

            return this;
        }

        internal StanCommandBusSubscriptionOptions Build()
        {
            var configOptions = _stanCommandBusSubscriptionConfigOptionsBuilder.Build();

            return new StanCommandBusSubscriptionOptions(configOptions);
        }
        
    }
}