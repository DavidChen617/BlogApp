using Domain.Posts;

namespace Domain.Comments;

public interface ICommentRepository
{
    void Add(Comment comment);
    Task<Comment?> GetByIdAsync(CommentId id, CancellationToken cancellationToken = default);
    void Delete(Comment comment);
    Task DeleteByPostIdAsync(PostId postId, CancellationToken cancellationToken = default);
}
