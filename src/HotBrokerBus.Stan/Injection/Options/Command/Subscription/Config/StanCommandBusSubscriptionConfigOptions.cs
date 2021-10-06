using System;
using System.Collections.Generic;

namespace HotBrokerBus.Stan.Injection.Options.Command.Subscription.Config
{
    public class StanCommandBusSubscriptionConfigOptions
    {
        public StanCommandBusSubscriptionConfigOptions(Dictionary<string, Tuple<Type, Type>> commands)
        {
            Commands = commands;
        }
        
        internal Dictionary<string, Tuple<Type, Type>> Commands { get; }
    }
}