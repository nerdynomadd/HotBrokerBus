using System;
using System.Collections.Generic;
using System.Linq;
using HotBrokerBus.Abstractions.Events;
using HotBrokerBus.Abstractions.Stan.Events;

namespace HotBrokerBus.Stan.Events
{
    public class StanEventBusStorage : IStanEventBusStorage
    {
        private readonly List<Tuple<string, IEvent>> _messages;

        public StanEventBusStorage()
        {
            _messages = new List<Tuple<string, IEvent>>();
        }

        public void AddEventMessage(string subject, IEvent @event)
        {
            _messages.Add(new Tuple<string, IEvent>(subject, @event));
        }

        public void RemoveEventMessage(Tuple<string, IEvent> message)
        {
            _messages.Remove(message);
        }

        public void ClearMessages()
        {
            _messages.Clear();
        }

        public List<Tuple<string, IEvent>> RetrieveEventMessages()
        {
            return _messages.ToList();
        }
    }
}