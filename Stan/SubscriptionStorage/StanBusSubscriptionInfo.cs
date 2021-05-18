using System;
using STAN.Client;

namespace Bus.Stan.SubscriptionStorage
{
    public class StanSubscriptionInfo
    {
        public StanSubscriptionInfo(string subscriptionName,
            string subject,
            string triggerName,
            string queueGroup,
            Types type,
            StanSubscriptionOptions stanSubscriptionOptions,
            Type subscriptionDescriber,
            Type subscriptionDescriberHandler)
        {
            SubscriptionName = subscriptionName;

            Subject = subject;

            TriggerName = triggerName;

            QueueGroup = queueGroup;

            Type = type;

            SubscriptionOptions = stanSubscriptionOptions;

            SubscriptionDescriber = subscriptionDescriber;

            SubscriptionDescriberHandler = subscriptionDescriberHandler;
        }

        public StanSubscriptionInfo(string subscriptionName,
            string subject,
            string triggerName,
            Types type,
            Type subscriptionDescriber,
            Type subscriptionDescriberHandler)
        {
            SubscriptionName = subscriptionName;

            Subject = subject;

            TriggerName = triggerName;

            Type = type;

            SubscriptionDescriber = subscriptionDescriber;

            SubscriptionDescriberHandler = subscriptionDescriberHandler;
        }

        public enum Types
        {
            Command,
            Event
        }

        public string SubscriptionName { get; }

        public string Subject { get; }

        public string TriggerName { get; }

        public string QueueGroup { get; }

        public Types Type { get; }

        public StanSubscriptionOptions SubscriptionOptions { get; }

        public Type SubscriptionDescriber { get; }

        public Type SubscriptionDescriberHandler { get; }
    }
}