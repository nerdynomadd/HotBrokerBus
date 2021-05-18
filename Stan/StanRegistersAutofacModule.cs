using Autofac;
using Autofac.Extras.Quartz;
using Bus.Abstractions.Stan;
using Bus.Abstractions.Stan.Commands;
using Bus.Abstractions.Stan.Events;
using Bus.Abstractions.Stan.Middleware;
using Bus.Middleware;
using Bus.Stan.Commands;
using Bus.Stan.Events;
using Bus.Stan.Jobs;
using Bus.Stan.Middleware;
using Bus.Stan.PersistentConnection;
using Bus.Stan.SubscriptionStorage;
using Quartz;
using STAN.Client;

namespace Bus.Stan
{
    public class StanRegistersAutofacModule : Module
    {
        private readonly string _clusterName;

        private readonly string _appName;

        private readonly StanOptions _stanOptions;

        public StanRegistersAutofacModule(string clusterName, string appName, StanOptions stanOptions)
        {
            _clusterName = clusterName;

            _appName = appName;

            _stanOptions = stanOptions;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new QuartzAutofacFactoryModule());

            builder.RegisterType<ReconnectJob>()
                .As<IJob>()
                .AsSelf();

            builder.Register(e =>
                {
                    var lifetimeScope = e.Resolve<ILifetimeScope>();

                    return new StanBusPersistentConnection(_clusterName, _appName, _stanOptions,
                        lifetimeScope);
                })
                .As<IStanBusPersistentConnection>()
                .SingleInstance();

            builder.Register(e => new StanBusSubscriptionsStorage())
                .As<IStanBusSubscriptionsStorage>()
                .SingleInstance();

            // Stan Bus Command Middlewares
            builder.RegisterType<StanBusCommandExecutionMiddleware>()
                .As<IStanBusCommandExecutionMiddleware>()
                .SingleInstance(); 
            
            builder.RegisterType<StanBusCommandParserMiddleware>()
                .As<IStanBusCommandParserMiddleware>()
                .SingleInstance();
            
            // Stan Bus Event Middlewares
            builder.RegisterType<StanBusEventParserMiddleware>()
                .As<IStanBusEventParserMiddleware>()
                .SingleInstance();
            
            builder.RegisterType<StanBusEventExecutionMiddleware>()
                .As<IStanBusEventExecutionMiddleware>()
                .SingleInstance();

            builder.Register(e =>
                {
                    var lifetimeScope = e.Resolve<ILifetimeScope>();
                    
                    var middlewareStorage = new StanBusMiddlewareStorage(lifetimeScope);
                    
                    middlewareStorage.AddEventMiddleware<IStanBusEventParserMiddleware>(BusMiddlewarePriority.First);
                    
                    middlewareStorage.AddEventMiddleware<IStanBusEventExecutionMiddleware>(BusMiddlewarePriority.Last);
                    
                    middlewareStorage.AddCommandMiddleware<IStanBusCommandParserMiddleware>(BusMiddlewarePriority.First);
                    
                    middlewareStorage.AddCommandMiddleware<IStanBusCommandExecutionMiddleware>(BusMiddlewarePriority.Last);

                    return middlewareStorage;
                })
                .As<IStanBusMiddlewareStorage>()
                .SingleInstance();

            builder.RegisterModule(new StanBusCommandAutofacModule());

            builder.RegisterModule(new StanBusEventRegisterAutofacModule());
        }
    }
}