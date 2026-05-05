using Domain.Posts;
using Domain.Posts.Events;

namespace Domain.Tests.Posts;

public class PostTests
{
    [Fact]
    public void Create_ShouldReturnDraftPost_WithPostCreatedEvent()
    {
        var authorId = Guid.NewGuid();

        var post = Post.Create("Hello DDD", "Some content", authorId, "David");

        Assert.Equal(PostStatus.Draft, post.Status);
        Assert.Equal("Hello DDD", post.Title);
        var events = post.PopDomainEvents();
        Assert.Single(events);
        Assert.IsType<PostCreatedEvent>(events[0]);
    }

    [Fact]
    public void Publish_ShouldChangeStatusToPublished_WithPostPublishedEvent()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "Author");
        post.PopDomainEvents();

        post.Publish();

        Assert.Equal(PostStatus.Published, post.Status);
        var events = post.PopDomainEvents();
        Assert.Single(events);
        Assert.IsType<PostPublishedEvent>(events[0]);
    }

    [Fact]
    public void Publish_WhenAlreadyPublished_ShouldThrow()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "Author");
        post.Publish();

        Assert.Throws<InvalidOperationException>(() => post.Publish());
    }

    [Fact]
    public void Update_WhenDraft_ShouldUpdateTitleAndContent()
    {
        var post = Post.Create("Old Title", "Old Content", Guid.NewGuid(), "Author");

        post.Update("New Title", "New Content");

        Assert.Equal("New Title", post.Title);
        Assert.Equal("New Content", post.Content);
    }

    [Fact]
    public void Update_WhenPublished_ShouldThrow()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "Author");
        post.Publish();

        Assert.Throws<InvalidOperationException>(() => post.Update("New Title", "New Content"));
    }

    [Fact]
    public void Delete_ShouldProducePostDeletedEvent()
    {
        var post = Post.Create("Title", "Content", Guid.NewGuid(), "Author");
        post.PopDomainEvents();

        post.Delete();

        var events = post.PopDomainEvents();
        Assert.Single(events);
        var deleted = Assert.IsType<PostDeletedEvent>(events[0]);
        Assert.Equal(post.Id.Value, deleted.PostId);
    }
}
