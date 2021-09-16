using System;
using HotBrokerBus.Abstractions.Events;

namespace HotBrokerBus.Events
{
    public class Event : IEvent
    {
        public Guid Id { get; } = new();

        public DateTime CreationDate { get; } = DateTime.Now;
    }
}