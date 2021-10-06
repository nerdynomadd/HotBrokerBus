using System;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Subscription
{
    public interface IStanSubscriptionInfo
    {
        public enum Types
        {
            Command,
            Event
        }

        public string SubscriptionName { get; }

        public string Subject { get; }
        
        public string QueueGroup { get; }

        public Types Type { get; }

        public StanSubscriptionOptions SubscriptionOptions { get; }

        public Type SubscriptionDescriber { get; }

        public Type SubscriptionDescriberHandler { get; }
    }
}