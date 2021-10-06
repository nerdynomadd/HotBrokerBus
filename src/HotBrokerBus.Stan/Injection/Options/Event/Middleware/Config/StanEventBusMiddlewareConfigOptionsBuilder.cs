using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Middleware;
using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Abstractions.Stan.Events.Middleware;
using HotBrokerBus.Stan.Injection.Parameters.Event.Middleware;

namespace HotBrokerBus.Stan.Injection.Options.Event.Middleware.Config
{
    public class StanEventBusMiddlewareConfigOptionsBuilder
    {
        private readonly List<StanEventMiddlewareParameters> _stanEventMiddlewareParameters;

        private readonly Dictionary<string, Type> _typesToBind;
        
        public StanEventBusMiddlewareConfigOptionsBuilder(List<StanEventMiddlewareParameters> stanEventMiddlewareParameters)
        {
            _stanEventMiddlewareParameters = stanEventMiddlewareParameters;

            _typesToBind = new();
        }
        
        public StanEventBusMiddlewareConfigOptionsBuilder Bind<T>()
            where T: IStanEventBusMiddleware
        {
            var typeName = typeof(T).Name;

            if (!_typesToBind.ContainsKey(typeName))
            {
                _typesToBind.Add(typeName,  typeof(T));
            }

            return this;
        }

        internal StanEventBusMiddlewareConfigOptions Build()
        {
            Dictionary<Type, BusMiddlewarePriority> middlewaresMap = new();
            
            foreach (var middleware in _stanEventMiddlewareParameters)
            {
                if (!_typesToBind.ContainsKey(middleware.Type))
                {
                    throw new ArgumentException(
                        $"The type {middleware.Type} is not bound. Add options.Bind<${middleware.Type}>() after stanEventMiddlewareOptions.WithConfig().");
                }

                if (!middlewaresMap.ContainsKey(_typesToBind[middleware.Type]))
                {
                    middlewaresMap.Add(_typesToBind[middleware.Type], middleware.Priority);
                }
            }
            
            return new StanEventBusMiddlewareConfigOptions(middlewaresMap);
        }
        
    }
}