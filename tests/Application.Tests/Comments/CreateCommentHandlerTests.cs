using Application.Comments.Commands;
using Domain.Comments;
using Domain.Common;
using Domain.Posts;
using NSubstitute;

namespace Application.Tests.Comments;

public class CreateCommentHandlerTests
{
    private readonly IPostRepository _postRepository = Substitute.For<IPostRepository>();
    private readonly ICommentRepository _commentRepository = Substitute.For<ICommentRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Handle_WhenPostIsPublished_ShouldAddComment()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "David");
        post.Publish();
        post.PopDomainEvents();
        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        var handler = new CreateCommentHandler(_postRepository, _commentRepository, _unitOfWork);
        await handler.Handle(new CreateCommentCommand(post.Id.Value, Guid.NewGuid(), "Nice post!"));

        _commentRepository.Received(1).Add(Arg.Is<Comment>(c => c.Body == "Nice post!"));
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_WhenPostIsDraft_ShouldThrow()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "David");
        post.PopDomainEvents();
        _postRepository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        var handler = new CreateCommentHandler(_postRepository, _commentRepository, _unitOfWork);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.Handle(new CreateCommentCommand(post.Id.Value, Guid.NewGuid(), "Nice post!")));
    }
}
