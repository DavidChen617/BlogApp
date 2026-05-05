using Application.Posts.Commands;
using CoreMesh.Outbox.Abstractions;
using Domain.Common;
using Domain.Posts;
using Domain.Posts.Events;
using NSubstitute;

namespace Application.Tests.Posts;

public class PublishPostHandlerTests
{
    private readonly IPostRepository _repository = Substitute.For<IPostRepository>();
    private readonly IOutboxWriter _outboxWriter = Substitute.For<IOutboxWriter>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Handle_ShouldPublishPost_AndWritePostPublishedEventToOutbox()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "David");
        post.PopDomainEvents();
        _repository.GetByIdAsync(Arg.Any<PostId>()).Returns(post);

        var handler = new PublishPostHandler(_repository, _outboxWriter, _unitOfWork);
        await handler.Handle(new PublishPostCommand(post.Id.Value));

        Assert.Equal(PostStatus.Published, post.Status);
        await _outboxWriter.Received(1).AddAsync(
            Arg.Is<IEvent>(e => e is PostPublishedEvent),
            Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
