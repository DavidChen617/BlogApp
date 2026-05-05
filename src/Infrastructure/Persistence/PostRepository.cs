using Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class PostRepository(BlogDbContext dbContext) : IPostRepository
{
    public void Add(Post post) => dbContext.Posts.Add(post);

    public async Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken = default) =>
        await dbContext.Posts.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public void Update(Post post) => dbContext.Posts.Update(post);

    public void Delete(Post post) => dbContext.Posts.Remove(post);

    public async Task<(IReadOnlyList<Post> Items, int TotalCount)> GetPagedAsync(
        int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Posts.AsQueryable();
        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .OrderByDescending(p => p.CreatedAtUtc)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<IReadOnlyList<Post>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken = default) =>
        await dbContext.Posts.Where(p => p.AuthorId == authorId).ToListAsync(cancellationToken);
}
