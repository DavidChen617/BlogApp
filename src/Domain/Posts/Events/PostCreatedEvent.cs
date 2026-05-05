using CoreMesh.Outbox.Abstractions;

namespace Domain.Posts.Events;

[EventName("post-created")]
public sealed record PostCreatedEvent(Guid PostId, Guid AuthorId, string Title) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;
}
