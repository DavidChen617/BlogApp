using CoreMesh.Outbox.Abstractions;

namespace Domain.Posts.Events;

public sealed record PostDeletedEvent(Guid PostId) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;
}
