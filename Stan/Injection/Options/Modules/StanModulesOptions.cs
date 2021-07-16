using HotBrokerBus.Stan.Extensions.Options.Modules.Connection;

namespace HotBrokerBus.Stan.Extensions.Options.Modules
{
    public class StanModulesOptions
    {
        public StanModulesOptions(StanModulesConnectionOptions connectionOptions)
        {
            ConnectionOptions = connectionOptions;
        }
        
        internal StanModulesConnectionOptions ConnectionOptions { get; set; }
    }
}