using System;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Middleware;
using HotBrokerBus.Stan.Extensions.Options.Event.Event;
using HotBrokerBus.Stan.Extensions.Options.Event.Event.Config;
using HotBrokerBus.Stan.Extensions.Options.Event.Middleware;
using HotBrokerBus.Stan.Extensions.Parameters.Event;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotBrokerBus.Stan.Extensions.Options.Event
{
    public class StanCommandBusOptionsBuilder
    {
        private StanCommandParameters _stanCommandParameters;

        private StanCommandBusSubscriptionOptionsBuilder _stanCommandBusSubscriptionConfigOptionsBuilder;
        
        private StanCommandBusMiddlewareOptionsBuilder _stanCommandBusMiddlewareOptionsBuilder;

        public StanCommandBusOptionsBuilder(IServiceCollection serviceCollection)
        {
            _stanCommandParameters = new();

            _stanCommandBusSubscriptionConfigOptionsBuilder = new(_stanCommandParameters.Commands.Subscriptions, serviceCollection);

            _stanCommandBusMiddlewareOptionsBuilder = new(_stanCommandParameters.Middlewares);
        }

        public StanCommandBusOptionsBuilder ConfigureMiddlewares(Action<StanCommandBusMiddlewareOptionsBuilder> options)
        {
            options(_stanCommandBusMiddlewareOptionsBuilder);

            return this;
        }

        public StanCommandBusOptionsBuilder ConfigureSubscriptions(Action<StanCommandBusSubscriptionOptionsBuilder> options)
        {
            options(_stanCommandBusSubscriptionConfigOptionsBuilder);

            return this;
        }

        public StanCommandBusOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanCommandParameters);

            return this;
        }
        
        internal StanCommandBusOptions Build()
        {
            _stanCommandBusMiddlewareOptionsBuilder.AddMiddleware<IStanCommandBusParserMiddleware>(BusMiddlewarePriority.First);
         
            _stanCommandBusMiddlewareOptionsBuilder.AddMiddleware<IStanCommandBusExecutionMiddleware>(BusMiddlewarePriority.Last);

            var middlewares = _stanCommandBusMiddlewareOptionsBuilder.Build();

            var subscriptions = _stanCommandBusSubscriptionConfigOptionsBuilder.Build();
            
            return new StanCommandBusOptions(middlewares, subscriptions);
        }
    }
}