using CoreMesh.Outbox.Abstractions;

namespace Domain.Posts.Events;

[EventName("post-published")]
public sealed record PostPublishedEvent(Guid PostId) : IEvent
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;
}
