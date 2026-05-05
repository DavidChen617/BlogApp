using CoreMesh.Outbox.Abstractions;
using Domain.Comments;
using Domain.Posts;
using Domain.Posts.Events;

namespace Application.EventHandlers;

public sealed class PostDeletedEventHandler(ICommentRepository commentRepository) : IEventHandler<PostDeletedEvent>
{
    public async Task HandleAsync(PostDeletedEvent @event, CancellationToken cancellationToken = default)
    {
        await commentRepository.DeleteByPostIdAsync(PostId.From(@event.PostId), cancellationToken);
    }
}
