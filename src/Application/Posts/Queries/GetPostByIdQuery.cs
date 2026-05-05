using CoreMesh.Dispatching.Abstractions;
using Domain.Posts;

namespace Application.Posts.Queries;

public sealed record GetPostByIdQuery(Guid PostId) : IRequest<PostDto>;

public sealed class GetPostByIdHandler(IPostRepository repository)
    : IRequestHandler<GetPostByIdQuery, PostDto>
{
    public async Task<PostDto> Handle(GetPostByIdQuery request, CancellationToken cancellationToken = default)
    {
        var post = await repository.GetByIdAsync(PostId.From(request.PostId), cancellationToken)
            ?? throw new KeyNotFoundException($"Post {request.PostId} not found.");

        return new PostDto(post.Id.Value, post.Title, post.Content, post.AuthorId, post.AuthorName, post.Status, post.CreatedAtUtc);
    }
}
