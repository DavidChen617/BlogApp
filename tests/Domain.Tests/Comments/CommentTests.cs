using Domain.Comments;
using Domain.Posts;

namespace Domain.Tests.Comments;

public class CommentTests
{
    [Fact]
    public void Create_ShouldReturnCommentWithCorrectProperties()
    {
        var postId = PostId.New();
        var authorId = Guid.NewGuid();

        var comment = Comment.Create(postId, authorId, "Great post!");

        Assert.Equal(postId, comment.PostId);
        Assert.Equal(authorId, comment.AuthorId);
        Assert.Equal("Great post!", comment.Body);
        Assert.True(comment.CreatedAtUtc <= DateTime.UtcNow);
    }
}
