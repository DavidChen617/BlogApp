using CoreMesh.Outbox.Abstractions;
using Infrastructure.Persistence;

namespace Infrastructure.Messaging;

public sealed class EfCoreOutboxWriter(BlogDbContext dbContext) : IOutboxWriter
{
    public async Task AddAsync(IEvent @event, CancellationToken cancellationToken = default)
    {
        var message = OutboxMessage.Create(@event);
        await dbContext.OutboxMessages.AddAsync(message, cancellationToken);
    }
}
