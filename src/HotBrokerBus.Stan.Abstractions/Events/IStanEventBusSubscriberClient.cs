using HotBrokerBus.Abstractions.Events;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Events
{
    public interface IStanEventBusSubscriberClient
    {
        public IStanConnection Connection { get; set; }

        void Resume();

        void SetConnection(IStanConnection connection);
        
        void Publish(string subject, IEvent @event);

        public void Subscribe<T, TH>(string subject, string queueGroup)
            where T : IEvent
            where TH : IEventHandler<T>;

        void Subscribe<T, TH>(string subject, string queueGroup,
            StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler<T>;

        public void Close<T>(string subject)
            where T : IEvent;

        public void Close(string subject, string eventName);

        void CloseAll();

        public void Unsubscribe<T>(string subject)
            where T : IEvent;

        public void Unsubscribe(string subject, string eventName);

        void UnsubscribeAll();
    }
}