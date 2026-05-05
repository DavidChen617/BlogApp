using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Outbox.Abstractions;
using Domain.Posts;

namespace Application.Posts.Commands;

public sealed record PublishPostCommand(Guid PostId) : IRequest;

public sealed class PublishPostHandler(IPostRepository repository, IOutboxWriter outboxWriter)
    : IRequestHandler<PublishPostCommand>
{
    public async Task Handle(PublishPostCommand request, CancellationToken cancellationToken = default)
    {
        var postId = PostId.From(request.PostId);
        var post = await repository.GetByIdAsync(postId, cancellationToken)
            ?? throw new KeyNotFoundException($"Post {request.PostId} not found.");

        post.Publish();
        repository.Update(post);

        foreach (var domainEvent in post.PopDomainEvents())
            await outboxWriter.AddAsync(domainEvent, cancellationToken);
    }
}
