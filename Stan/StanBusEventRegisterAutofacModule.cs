using Autofac;
using Bus.Abstractions.Stan;
using Bus.Abstractions.Stan.Events;
using Bus.Stan.Events;
using Microsoft.Extensions.Logging;

namespace Bus.Stan
{
    public class StanBusEventRegisterAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(e => new StanBusEventStorage())
                .AsSelf()
                .As<IStanBusEventStorage>()
                .SingleInstance();

            builder.Register(e =>
                {
                    var stanBusPersistentConnection = e.Resolve<IStanBusPersistentConnection>();
                    var stanBusSubscriptionsStorage = e.Resolve<IStanBusSubscriptionsStorage>();
                    var stanIntegrationEventsStorage = e.Resolve<IStanBusEventStorage>();
                    var lifetimeScope = e.Resolve<ILifetimeScope>();
                    var logger = e.Resolve<ILogger<StanBusEventRegister>>();

                    return new StanBusEventRegister(stanBusPersistentConnection, stanBusSubscriptionsStorage,
                        stanIntegrationEventsStorage, lifetimeScope, logger);
                })
                .As<IStanBusEventRegister>()
                .AsSelf()
                .SingleInstance();
        }
    }
}