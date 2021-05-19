using System.Collections.Generic;
using System.Linq;
using HotBrokerBus.Abstractions.Commands;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan;
using STAN.Client;

namespace HotBrokerBus.Stan.SubscriptionStorage
{
    public class StanBusSubscriptionsStorage : IStanBusSubscriptionsStorage
    {
        private readonly IList<StanSubscriptionInfo> _subscriptionInfos;

        public StanBusSubscriptionsStorage()
        {
            _subscriptionInfos = new List<StanSubscriptionInfo>();
        }

        public void AddEventSubscription<T, TH>(string subscriptionName, string subject, string eventName,
            string queueGroup, StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler
        {
            _subscriptionInfos.Add(new StanSubscriptionInfo(subscriptionName, subject, eventName, queueGroup,
                StanSubscriptionInfo.Types.Event, subscriptionOptions, typeof(T), typeof(TH)));
        }

        public void AddCommandSubscription<T, TH>(string subscriptionName, string subject, string triggerName)
            where T : ICommand
            where TH : ICommandHandler
        {
            _subscriptionInfos.Add(new StanSubscriptionInfo(subscriptionName, subject, triggerName,
                StanSubscriptionInfo.Types.Command, typeof(T), typeof(TH)));
        }

        public List<StanSubscriptionInfo> RetrieveCommandSubscriptions()
        {
            return _subscriptionInfos.Where(e => e.Type == StanSubscriptionInfo.Types.Command).ToList();
        }

        public List<StanSubscriptionInfo> RetrieveEventSubscriptions()
        {
            return _subscriptionInfos.Where(e => e.Type == StanSubscriptionInfo.Types.Event).ToList();
        }

        public void RemoveSubscription(string subscriptionName)
        {
            _subscriptionInfos.Remove(_subscriptionInfos.First(e => e.SubscriptionName == subscriptionName));
        }

        public bool HasSubscription(string subscriptionName)
        {
            return _subscriptionInfos.Any(e => e.SubscriptionName == subscriptionName);
        }
    }
}