namespace HotBrokerBus.Stan.Extensions.Options.Modules.Connection.StanOptions
{
    public class StanModulesConnectionStanOptionsOptions
    {
        public StanModulesConnectionStanOptionsOptions(STAN.Client.StanOptions stanOptions)
        {
            StanOptions = stanOptions;
        }
        
        internal STAN.Client.StanOptions StanOptions { get; }
    }
}