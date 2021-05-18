using Autofac;
using Bus.Abstractions.Stan;
using Bus.Abstractions.Stan.Commands;
using Bus.Stan.Commands;
using Microsoft.Extensions.Logging;

namespace Bus.Stan
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