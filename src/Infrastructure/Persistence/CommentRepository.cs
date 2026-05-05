using Domain.Comments;
using Domain.Posts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class CommentRepository(BlogDbContext dbContext) : ICommentRepository
{
    public void Add(Comment comment) => dbContext.Comments.Add(comment);

    public async Task<Comment?> GetByIdAsync(CommentId id, CancellationToken cancellationToken = default) =>
        await dbContext.Comments.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public void Delete(Comment comment) => dbContext.Comments.Remove(comment);

    public async Task DeleteByPostIdAsync(PostId postId, CancellationToken cancellationToken = default)
    {
        await dbContext.Comments
            .Where(c => c.PostId == postId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}
