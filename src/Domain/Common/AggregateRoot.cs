using CoreMesh.Outbox.Abstractions;

namespace Domain.Common;

public abstract class AggregateRoot<TId> : Entity<TId>
{
    public IReadOnlyList<IEvent> PopDomainEvents()
    {
        var events = DomainEvents.ToList();
        ClearDomainEvents();
        return events;
    }
}
