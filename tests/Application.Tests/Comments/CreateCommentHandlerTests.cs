using Application.Comments.Commands;
using Domain.Comments;
using Domain.Posts;
using NSubstitute;

namespace Application.Tests.Comments;

public class CreateCommentHandlerTests
{
    private readonly IPostRepository _postRepository = Substitute.For<IPostRepository>();
    private readonly ICommentRepository _commentRepository = Substitute.For<ICommentRepository>();

    [Fact]
    public async Task Handle_WhenPostIsPublished_ShouldAddComment()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "David");
        post.Publish();
        post.PopDomainEvents();
        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        var handler = new CreateCommentHandler(_postRepository, _commentRepository);
        await handler.Handle(new CreateCommentCommand(post.Id.Value, Guid.NewGuid(), "Nice post!"));

        _commentRepository.Received(1).Add(Arg.Is<Comment>(c => c.Body == "Nice post!"));
    }

    [Fact]
    public async Task Handle_WhenPostIsDraft_ShouldThrow()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "David");
        post.PopDomainEvents();
        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        var handler = new CreateCommentHandler(_postRepository, _commentRepository);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CreateCommentCommand(post.Id.Value, Guid.NewGuid(), "Nice post!")));
    }
}
