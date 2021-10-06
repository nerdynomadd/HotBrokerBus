using System;
using HotBrokerBus.Abstractions.Stan.Subscription;
using STAN.Client;

namespace HotBrokerBus.Stan.SubscriptionStorage
{
    public class StanSubscriptionInfo : IStanSubscriptionInfo
    {
        public StanSubscriptionInfo(string subscriptionName,
            string subject,
            string queueGroup,
            IStanSubscriptionInfo.Types type,
            StanSubscriptionOptions stanSubscriptionOptions,
            Type subscriptionDescriber,
            Type subscriptionDescriberHandler)
        {
            SubscriptionName = subscriptionName;

            Subject = subject;
            
            QueueGroup = queueGroup;

            Type = type;

            SubscriptionOptions = stanSubscriptionOptions;

            SubscriptionDescriber = subscriptionDescriber;

            SubscriptionDescriberHandler = subscriptionDescriberHandler;
        }

        public StanSubscriptionInfo(string subscriptionName,
            string subject,
            IStanSubscriptionInfo.Types type,
            Type subscriptionDescriber,
            Type subscriptionDescriberHandler)
        {
            SubscriptionName = subscriptionName;

            Subject = subject;
            
            Type = type;

            SubscriptionDescriber = subscriptionDescriber;

            SubscriptionDescriberHandler = subscriptionDescriberHandler;
        }

        public string SubscriptionName { get; }

        public string Subject { get; }
        
        public string QueueGroup { get; }

        public IStanSubscriptionInfo.Types Type { get; }

        public StanSubscriptionOptions SubscriptionOptions { get; }

        public Type SubscriptionDescriber { get; }

        public Type SubscriptionDescriberHandler { get; }
    }
}