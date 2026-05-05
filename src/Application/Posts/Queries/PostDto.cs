using Domain.Posts;

namespace Application.Posts.Queries;

public sealed record PostDto(
    Guid Id,
    string Title,
    string Content,
    Guid AuthorId,
    string AuthorName,
    PostStatus Status,
    DateTime CreatedAtUtc);

public sealed record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int Page, int PageSize);
