using System;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Abstractions.Stan.Events;
using HotBrokerBus.Abstractions.Stan.Middleware;
using HotBrokerBus.Stan.Commands;
using HotBrokerBus.Stan.Events;
using HotBrokerBus.Stan.Extensions.Options.Event;
using HotBrokerBus.Stan.Extensions.Options.Modules;
using HotBrokerBus.Stan.HostedService;
using HotBrokerBus.Stan.Jobs;
using HotBrokerBus.Stan.Middleware;
using HotBrokerBus.Stan.PersistentConnection;
using HotBrokerBus.Stan.SubscriptionStorage;
using Microsoft.Extensions.DependencyInjection;
using Quartz;

namespace HotBrokerBus.Stan.Extensions
{
    public static class StanDependencyInjectionExtensions
    {
        private static void _addQuartzSchedulerModule(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionScopedJobFactory();
            });
        }

        public static IServiceCollection AddStanModules(this IServiceCollection serviceCollection, Action<StanModulesOptionsBuilder> options)
        {
            StanModulesOptionsBuilder optionsBuilder = new();

            options(optionsBuilder);
            
            serviceCollection.AddSingleton(optionsBuilder.Build());
            
            serviceCollection.AddSingleton<IReconnectJob, ReconnectJob>();

            serviceCollection.AddSingleton<IStanBusSubscriptionsStorage, StanBusSubscriptionsStorage>();

            serviceCollection.AddSingleton<IStanBusPersistentConnection, StanBusPersistentConnection>();

            serviceCollection.AddHostedService<StanBusHostedService>();

            serviceCollection._addQuartzSchedulerModule();

            return serviceCollection;
        }

        public static IServiceCollection AddStanEventBus(this IServiceCollection serviceCollection, Action<StanEventBusOptionsBuilder> options)
        {
            StanEventBusOptionsBuilder optionsBuilder = new(serviceCollection);

            options(optionsBuilder);
            
            serviceCollection.AddSingleton(optionsBuilder.Build());

            serviceCollection.AddSingleton<IStanEventBusStorage, StanEventBusStorage>();

            serviceCollection.AddSingleton<IStanEventBusRegister, StanEventBusRegister>();
            
            serviceCollection.AddSingleton<IStanEventBusMiddlewareStorage, StanEventBusMiddlewareStorage>();
            
            serviceCollection.AddTransient<IStanEventBusParserMiddleware, StanEventBusParserMiddleware>();
            serviceCollection.AddTransient<IStanEventBusExecutionMiddleware, StanEventBusExecutionMiddleware>();

            return serviceCollection;
        }

        public static IServiceCollection AddStanCommandBus(this IServiceCollection serviceCollection, Action<StanCommandBusOptionsBuilder> options)
        {
            StanCommandBusOptionsBuilder optionsBuilder = new(serviceCollection);

            options(optionsBuilder);
            
            serviceCollection.AddSingleton(optionsBuilder.Build());

            serviceCollection.AddSingleton<IStanCommandBusRegister, StanCommandBusRegister>();

            serviceCollection.AddSingleton<IStanCommandBusMiddlewareStorage, StanCommandBusMiddlewareStorage>();
            
            serviceCollection.AddTransient<IStanCommandBusParserMiddleware, StanCommandBusParserMiddleware>();
            serviceCollection.AddTransient<IStanCommandBusExecutionMiddleware, StanCommandBusExecutionMiddleware>();

            return serviceCollection;
        }
    }
}