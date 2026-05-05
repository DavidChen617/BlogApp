using Application.Posts.Commands;
using CoreMesh.Outbox.Abstractions;
using Domain.Common;
using Domain.Posts;
using Domain.Posts.Events;
using NSubstitute;

namespace Application.Tests.Posts;

public class CreatePostHandlerTests
{
    private readonly IPostRepository _repository = Substitute.For<IPostRepository>();
    private readonly IOutboxWriter _outboxWriter = Substitute.For<IOutboxWriter>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

    [Fact]
    public async Task Handle_ShouldAddPost_AndWritePostCreatedEventToOutbox()
    {
        var handler = new CreatePostHandler(_repository, _outboxWriter, _unitOfWork);
        var command = new CreatePostCommand("Title", "Content", Guid.NewGuid(), "David");

        await handler.Handle(command);

        _repository.Received(1).Add(Arg.Is<Post>(p =>
            p.Title == "Title" &&
            p.Status == PostStatus.Draft));

        await _outboxWriter.Received(1).AddAsync(
            Arg.Is<IEvent>(e => e is PostCreatedEvent),
            Arg.Any<CancellationToken>());

        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
