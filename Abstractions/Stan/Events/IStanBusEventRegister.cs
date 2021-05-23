using HotBrokerBus.Abstractions.Events;
using STAN.Client;

namespace HotBrokerBus.Abstractions.Stan.Events
{
    public interface IStanBusEventRegister
    {
        public IStanConnection Connection { get; set; }

        internal void Resume();

        internal void SetConnection(IStanConnection connection);

        void Publish(string subject, IEvent @event);

        void Publish(string subject, string eventName, IEvent @event);

        public void Subscribe<T, TH>(string subject, string queueGroup)
            where T : IEvent
            where TH : IEventHandler<T>;
        
        void Subscribe<T, TH>(string subject, string queueGroup, StanSubscriptionOptions subscriptionOptions)
            where T : IEvent
            where TH : IEventHandler<T>;

        public void Subscribe<T, TH>(string subject, string eventName, string queueGroup)
            where T : IEvent
            where TH : IEventHandler<T>;

        void Subscribe<T, TH>(string subject, string eventName, string queueGroup,
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