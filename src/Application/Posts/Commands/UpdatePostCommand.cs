using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Validation.Abstractions;
using CoreMesh.Validation.Abstractions.Extensions;
using Domain.Posts;

namespace Application.Posts.Commands;

public sealed record UpdatePostCommand(Guid PostId, string Title, string Content)
    : IRequest, IValidatable<UpdatePostCommand>
{
    public void ConfigureValidateRules(IValidationBuilder<UpdatePostCommand> builder)
    {
        builder.For(x => x.Title).NotNull().NotEmpty().MaxLength(200);
        builder.For(x => x.Content).NotNull().NotEmpty();
    }
}

public sealed class UpdatePostHandler(IPostRepository repository)
    : IRequestHandler<UpdatePostCommand>
{
    public async Task Handle(UpdatePostCommand request, CancellationToken cancellationToken = default)
    {
        var postId = PostId.From(request.PostId);
        var post = await repository.GetByIdAsync(postId, cancellationToken)
            ?? throw new KeyNotFoundException($"Post {request.PostId} not found.");

        post.Update(request.Title, request.Content);
        repository.Update(post);
    }
}
