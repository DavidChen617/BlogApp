using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Outbox.Abstractions;
using CoreMesh.Validation.Abstractions;
using CoreMesh.Validation.Abstractions.Extensions;
using Domain.Posts;

namespace Application.Posts.Commands;

public sealed record CreatePostCommand(string Title, string Content, Guid AuthorId, string AuthorName)
    : IRequest, IValidatable<CreatePostCommand>
{
    public void ConfigureValidateRules(IValidationBuilder<CreatePostCommand> builder)
    {
        builder.For(x => x.Title).NotNull().NotEmpty().MaxLength(200);
        builder.For(x => x.Content).NotNull().NotEmpty();
        builder.For(x => x.AuthorName).NotNull().NotEmpty();
    }
}

public sealed class CreatePostHandler(IPostRepository repository, IOutboxWriter outboxWriter)
    : IRequestHandler<CreatePostCommand>
{
    public async Task Handle(CreatePostCommand request, CancellationToken cancellationToken = default)
    {
        var post = Post.Create(request.Title, request.Content, request.AuthorId, request.AuthorName);
        repository.Add(post);

        foreach (var domainEvent in post.PopDomainEvents())
            await outboxWriter.AddAsync(domainEvent, cancellationToken);
    }
}
