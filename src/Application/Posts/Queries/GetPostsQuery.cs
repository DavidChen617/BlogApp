using CoreMesh.Dispatching.Abstractions;
using Domain.Posts;

namespace Application.Posts.Queries;

public sealed record GetPostsQuery(int Page = 1, int PageSize = 20) : IRequest<PagedResult<PostDto>>;

public sealed class GetPostsHandler(IPostRepository repository)
    : IRequestHandler<GetPostsQuery, PagedResult<PostDto>>
{
    public async Task<PagedResult<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken = default)
    {
        var (items, totalCount) = await repository.GetPagedAsync(request.Page, request.PageSize, cancellationToken);

        var dtos = items.Select(p =>
            new PostDto(p.Id.Value, p.Title, p.Content, p.AuthorId, p.AuthorName, p.Status, p.CreatedAtUtc))
            .ToList();

        return new PagedResult<PostDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}
