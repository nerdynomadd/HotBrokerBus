using HotBrokerBus.Stan.Extensions.Options.Modules.Connection;
using HotBrokerBus.Stan.Extensions.Options.Modules.HostedService;

namespace HotBrokerBus.Stan.Extensions.Options.Modules
{
    public class StanModulesOptions
    {
        public StanModulesOptions(StanModulesConnectionOptions connectionOptions,
            StanModulesHostedServiceOptions hostedServiceOptions)
        {
            ConnectionOptions = connectionOptions;

            HostedServiceOptions = hostedServiceOptions;
        }
        
        internal StanModulesConnectionOptions ConnectionOptions { get; set; }
        
        internal StanModulesHostedServiceOptions HostedServiceOptions { get; set; }
    }
}