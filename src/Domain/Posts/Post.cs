using Domain.Common;
using Domain.Posts.Events;

namespace Domain.Posts;

public sealed class Post : AggregateRoot<PostId>
{
    public string Title { get; private set; } = default!;
    public string Content { get; private set; } = default!;
    public Guid AuthorId { get; private set; }
    public string AuthorName { get; private set; } = default!;
    public PostStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }

    private Post() { }

    public static Post Create(string title, string content, Guid authorId, string authorName)
    {
        var post = new Post
        {
            Id = PostId.New(),
            Title = title,
            Content = content,
            AuthorId = authorId,
            AuthorName = authorName,
            Status = PostStatus.Draft,
            CreatedAtUtc = DateTime.UtcNow
        };

        post.AddDomainEvent(new PostCreatedEvent(post.Id.Value, authorId, title));
        return post;
    }

    public void Publish()
    {
        if (Status == PostStatus.Published)
            throw new InvalidOperationException(PostErrors.AlreadyPublished);

        Status = PostStatus.Published;
        AddDomainEvent(new PostPublishedEvent(Id.Value));
    }

    public void Update(string title, string content)
    {
        if (Status == PostStatus.Published)
            throw new InvalidOperationException(PostErrors.CannotModifyPublished);

        Title = title;
        Content = content;
    }

    public void Delete()
    {
        AddDomainEvent(new PostDeletedEvent(Id.Value));
    }

    public void UpdateAuthorName(string authorName)
    {
        AuthorName = authorName;
    }
}
