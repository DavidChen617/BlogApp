using CoreMesh.Dispatching.Abstractions;
using Domain.Comments;
using Domain.Common;

namespace Application.Comments.Commands;

public sealed record DeleteCommentCommand(Guid CommentId) : IRequest;

public sealed class DeleteCommentHandler(ICommentRepository repository, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteCommentCommand>
{
    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken = default)
    {
        var comment = await repository.GetByIdAsync(CommentId.From(request.CommentId), cancellationToken)
            ?? throw new KeyNotFoundException($"Comment {request.CommentId} not found.");

        repository.Delete(comment);

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
