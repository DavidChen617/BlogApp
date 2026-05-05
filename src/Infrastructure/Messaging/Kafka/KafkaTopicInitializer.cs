using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Messaging.Kafka;

public sealed class KafkaTopicInitializer(IConfiguration configuration) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var bootstrapServers = configuration["Kafka:BootstrapServers"]!;
        using var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = bootstrapServers }).Build();

        var topics = new[]
        {
            KafkaTopics.PostCreated,
            KafkaTopics.PostPublished,
            KafkaTopics.PostDeleted,
            KafkaTopics.AuthorNameChanged
        };

        try
        {
            await adminClient.CreateTopicsAsync(topics.Select(t => new TopicSpecification
            {
                Name = t,
                NumPartitions = 1,
                ReplicationFactor = 1
            }));
        }
        catch (CreateTopicsException ex)
            when (ex.Results.All(r => r.Error.Code == ErrorCode.TopicAlreadyExists))
        {
        }
    }
}
