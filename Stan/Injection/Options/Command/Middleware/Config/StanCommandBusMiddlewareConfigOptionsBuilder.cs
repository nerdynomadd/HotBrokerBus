using System;
using System.Collections.Generic;
using HotBrokerBus.Abstractions.Middleware.Commands;
using HotBrokerBus.Abstractions.Middleware.Events;
using HotBrokerBus.Middleware;
using HotBrokerBus.Stan.Extensions.Parameters.Event.Middleware;

namespace HotBrokerBus.Stan.Extensions.Options.Event.Middleware
{
    public class StanCommandBusMiddlewareConfigOptionsBuilder
    {
        private readonly List<StanEventMiddlewareParameters> _stanEventMiddlewareParameters;

        private readonly Dictionary<string, Type> _typesToBind;
        
        public StanCommandBusMiddlewareConfigOptionsBuilder(List<StanEventMiddlewareParameters> stanEventMiddlewareParameters)
        {
            _stanEventMiddlewareParameters = stanEventMiddlewareParameters;

            _typesToBind = new();
        }
        
        public StanCommandBusMiddlewareConfigOptionsBuilder Bind<T>()
            where T: ICommandBusMiddleware
        {
            var typeName = typeof(T).Name;

            if (!_typesToBind.ContainsKey(typeName))
            {
                _typesToBind.Add(typeName,  typeof(T));
            }

            return this;
        }

        internal StanCommandBusMiddlewareConfigOptions Build()
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
            
            return new StanCommandBusMiddlewareConfigOptions(middlewaresMap);
        }
        
    }
}