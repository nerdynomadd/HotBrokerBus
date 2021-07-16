using HotBrokerBus.Stan.Extensions.Options.Modules.Connection.StanOptions;

namespace HotBrokerBus.Stan.Extensions.Options.Modules.Connection
{
    public class StanModulesConnectionOptions
    {
        public StanModulesConnectionOptions(string clusterName, string appName, StanModulesConnectionStanOptionsOptions stanOptionsOptions)
        {
            ClusterName = clusterName;

            AppName = appName;

            StanOptionsOptions = stanOptionsOptions;
        }
        
        internal string ClusterName { get; }
        
        internal string AppName { get; }
        
        internal StanModulesConnectionStanOptionsOptions StanOptionsOptions { get; }
    }
}