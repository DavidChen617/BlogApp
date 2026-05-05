using CoreMesh.Outbox.Abstractions;

namespace Domain.Common;

public abstract class Entity<TId>
{
    private readonly List<IEvent> _domainEvents = [];

    public TId Id { get; protected set; } = default!;
    public IReadOnlyList<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IEvent domainEvent) => _domainEvents.Add(domainEvent);

    protected void ClearDomainEvents() => _domainEvents.Clear();
}
