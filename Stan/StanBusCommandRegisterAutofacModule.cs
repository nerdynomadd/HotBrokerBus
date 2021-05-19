using Autofac;
using HotBrokerBus.Abstractions.Stan;
using HotBrokerBus.Abstractions.Stan.Commands;
using HotBrokerBus.Stan.Commands;
using Microsoft.Extensions.Logging;

namespace HotBrokerBus.Stan
{
    public class StanBusCommandAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(e =>
                {
                    var stanBusPersistentConnection = e.Resolve<IStanBusPersistentConnection>();
                    var stanBusSubscriptionsStorage = e.Resolve<IStanBusSubscriptionsStorage>();
                    var lifetimeScope = e.Resolve<ILifetimeScope>();
                    var logger = e.Resolve<ILogger<StanBusCommandRegister>>();

                    return new StanBusCommandRegister(stanBusPersistentConnection, stanBusSubscriptionsStorage,
                        lifetimeScope, logger);
                })
                .As<IStanBusCommandRegister>()
                .AsSelf()
                .SingleInstance();
        }
    }
}