using System;

namespace HotBrokerBus.Abstractions.Events
{
    public class Event : IEvent
    {
        public Guid Id { get; } = new();

        public DateTime CreationDate { get; } = DateTime.Now;
    }
}