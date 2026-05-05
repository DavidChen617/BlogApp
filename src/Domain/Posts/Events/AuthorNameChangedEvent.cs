using CoreMesh.Outbox.Abstractions;

namespace Domain.Posts.Events;

[EventName("author-name-changed")]
public sealed record AuthorNameChangedEvent(Guid AuthorId, string NewAuthorName) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;
}
