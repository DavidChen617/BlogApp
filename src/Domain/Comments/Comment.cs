using Domain.Common;
using Domain.Posts;

namespace Domain.Comments;

public sealed class Comment : AggregateRoot<CommentId>
{
    public PostId PostId { get; private set; } = default!;
    public Guid AuthorId { get; private set; }
    public string Body { get; private set; } = default!;
    public DateTime CreatedAtUtc { get; private set; }

    private Comment() { }

    public static Comment Create(PostId postId, Guid authorId, string body)
    {
        return new Comment
        {
            Id = CommentId.New(),
            PostId = postId,
            AuthorId = authorId,
            Body = body,
            CreatedAtUtc = DateTime.UtcNow
        };
    }
}
