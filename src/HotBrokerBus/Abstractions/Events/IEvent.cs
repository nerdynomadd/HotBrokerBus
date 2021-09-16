using System;

namespace HotBrokerBus.Abstractions.Events
{
    public interface IEvent
    {
        Guid Id { get; }

        DateTime CreationDate { get; }
    }
}