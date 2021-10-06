using System;
using HotBrokerBus.Abstractions.Middleware;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Abstractions.Stan.Events.Middleware;
using HotBrokerBus.Stan.Injection.Options.Event.Middleware;
using HotBrokerBus.Stan.Injection.Options.Event.Subscription;
using HotBrokerBus.Stan.Injection.Parameters.Event;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HotBrokerBus.Stan.Injection.Options.Event
{
    public class StanEventBusOptionsBuilder
    {
        private StanEventParameters _stanEventParameters;

        private StanEventBusSubscriptionOptionsBuilder _stanEventBusSubscriptionConfigOptionsBuilder;
        
        private StanEventBusMiddlewareOptionsBuilder _stanEventBusMiddlewareOptionsBuilder;

        public StanEventBusOptionsBuilder(IServiceCollection serviceCollection)
        {
            _stanEventParameters = new();

            _stanEventBusSubscriptionConfigOptionsBuilder = new(_stanEventParameters.Events.Subscriptions, serviceCollection);

            _stanEventBusMiddlewareOptionsBuilder = new(_stanEventParameters.Middlewares);
        }

        public StanEventBusOptionsBuilder ConfigureMiddlewares(Action<StanEventBusMiddlewareOptionsBuilder> options)
        {
            options(_stanEventBusMiddlewareOptionsBuilder);

            return this;
        }

        public StanEventBusOptionsBuilder ConfigureSubscriptions(Action<StanEventBusSubscriptionOptionsBuilder> options)
        {
            options(_stanEventBusSubscriptionConfigOptionsBuilder);

            return this;
        }

        public StanEventBusOptionsBuilder WithConfig(IConfigurationSection configurationSection)
        {
            configurationSection.Bind(_stanEventParameters);

            return this;
        }
        
        public StanEventBusOptions Build()
        {
            _stanEventBusMiddlewareOptionsBuilder.AddMiddleware<IStanEventBusParserMiddleware>(BusMiddlewarePriority.First);
            
            _stanEventBusMiddlewareOptionsBuilder.AddMiddleware<IStanEventBusExecutionMiddleware>(BusMiddlewarePriority.Last);
            
            var middlewares = _stanEventBusMiddlewareOptionsBuilder.Build();

            var subscriptions = _stanEventBusSubscriptionConfigOptionsBuilder.Build();
            
            return new StanEventBusOptions(middlewares, subscriptions);
        }
    }
}