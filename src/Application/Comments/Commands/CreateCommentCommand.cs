using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Validation.Abstractions;
using CoreMesh.Validation.Abstractions.Extensions;
using Domain.Comments;
using Domain.Posts;

namespace Application.Comments.Commands;

public sealed record CreateCommentCommand(Guid PostId, Guid AuthorId, string Body)
    : IRequest, IValidatable<CreateCommentCommand>
{
    public void ConfigureValidateRules(IValidationBuilder<CreateCommentCommand> builder)
    {
        builder.For(x => x.Body).NotNull().NotEmpty().MaxLength(2000);
    }
}

public sealed class CreateCommentHandler(IPostRepository postRepository, ICommentRepository commentRepository)
    : IRequestHandler<CreateCommentCommand>
{
    public async Task Handle(CreateCommentCommand request, CancellationToken cancellationToken = default)
    {
        var postId = PostId.From(request.PostId);
        var post = await postRepository.GetByIdAsync(postId, cancellationToken)
            ?? throw new KeyNotFoundException($"Post {request.PostId} not found.");

        if (post.Status != PostStatus.Published)
            throw new InvalidOperationException("Cannot comment on a non-published post.");

        var comment = Comment.Create(postId, request.AuthorId, request.Body);
        commentRepository.Add(comment);
    }
}
