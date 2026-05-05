using CoreMesh.Outbox.Abstractions;
using Domain.Posts;
using Domain.Posts.Events;

namespace Application.EventHandlers;

public sealed class AuthorNameChangedEventHandler(IPostRepository postRepository) : IEventHandler<AuthorNameChangedEvent>
{
    public async Task HandleAsync(AuthorNameChangedEvent @event, CancellationToken cancellationToken = default)
    {
        var posts = await postRepository.GetByAuthorIdAsync(@event.AuthorId, cancellationToken);
        foreach (var post in posts)
        {
            post.UpdateAuthorName(@event.NewAuthorName);
            postRepository.Update(post);
        }
    }
}
