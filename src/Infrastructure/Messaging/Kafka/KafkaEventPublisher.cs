using Confluent.Kafka;
using CoreMesh.Outbox.Abstractions;

namespace Infrastructure.Messaging.Kafka;

internal static class KafkaTopics
{
    public const string PostCreated = "post-created";
    public const string PostPublished = "post-published";
    public const string PostDeleted = "post-deleted";
    public const string AuthorNameChanged = "author-name-changed";

    public static string FromEventType(string eventType) => eventType switch
    {
        "post-created" => PostCreated,
        "post-published" => PostPublished,
        "post-deleted" => PostDeleted,
        "author-name-changed" => AuthorNameChanged,
        _ => throw new InvalidOperationException($"Unknown event type: {eventType}")
    };
}

public sealed class KafkaEventPublisher(IProducer<string, string> producer) : IEventPublisher
{
    public async Task PublishAsync(OutboxMessage message, CancellationToken cancellationToken = default)
    {
        var topic = KafkaTopics.FromEventType(message.EventType);
        await producer.ProduceAsync(topic, new Message<string, string>
        {
            Key = message.EventType,
            Value = message.Payload,
            Timestamp = new Timestamp(message.OccurredAtUtc)
        }, cancellationToken);
    }
}
