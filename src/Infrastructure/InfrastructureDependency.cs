using Application.EventHandlers;
using Confluent.Kafka;
using CoreMesh.Outbox.Extensions;
using Domain.Comments;
using Domain.Common;
using Domain.Posts;
using Infrastructure.Messaging;
using Infrastructure.Messaging.Kafka;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureDependency
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<BlogDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, EfUnitOfWork>();
        services.AddScoped<ITransactionUnitOfWork, EfUnitOfWork>();

        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<ICommentRepository, CommentRepository>();

        var bootstrapServers = configuration["Kafka:BootstrapServers"]!;
        var groupId = configuration["Kafka:GroupId"] ?? "blog-api";

        services.AddSingleton<IProducer<string, string>>(_ =>
            new ProducerBuilder<string, string>(new ProducerConfig
            {
                BootstrapServers = bootstrapServers,
                Acks = Acks.All
            }).Build());

        services.AddSingleton<IConsumer<string, string>>(_ =>
            new ConsumerBuilder<string, string>(new ConsumerConfig
            {
                BootstrapServers = bootstrapServers,
                GroupId = groupId,
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            }).Build());

        services.AddHostedService<KafkaTopicInitializer>();

        services.AddCoreMeshOutbox(
            [typeof(PostDeletedEventHandler).Assembly, typeof(Domain.Posts.Events.PostCreatedEvent).Assembly],
            options =>
            {
                options.AddOutboxStore<EfCoreOutboxStore>()
                       .AddOutboxWriter<EfCoreOutboxWriter>()
                       .AddMessageQueue<KafkaEventPublisher, KafkaMessageSubscriber>()
                       .WithConsumer();
            });

        return services;
    }
}
