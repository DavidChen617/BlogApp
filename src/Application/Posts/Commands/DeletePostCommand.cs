using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Outbox.Abstractions;
using Domain.Posts;

namespace Application.Posts.Commands;

public sealed record DeletePostCommand(Guid PostId) : IRequest;

public sealed class DeletePostHandler(IPostRepository repository, IOutboxWriter outboxWriter)
    : IRequestHandler<DeletePostCommand>
{
    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken = default)
    {
        var postId = PostId.From(request.PostId);
        var post = await repository.GetByIdAsync(postId, cancellationToken)
            ?? throw new KeyNotFoundException($"Post {request.PostId} not found.");

        post.Delete();
        repository.Delete(post);

        foreach (var domainEvent in post.PopDomainEvents())
            await outboxWriter.AddAsync(domainEvent, cancellationToken);
    }
}
