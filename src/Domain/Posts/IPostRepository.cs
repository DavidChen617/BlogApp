namespace Domain.Posts;

public interface IPostRepository
{
    void Add(Post post);
    Task<Post?> GetByIdAsync(PostId id, CancellationToken cancellationToken = default);
    void Update(Post post);
    void Delete(Post post);
    Task<(IReadOnlyList<Post> Items, int TotalCount)> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Post>> GetByAuthorIdAsync(Guid authorId, CancellationToken cancellationToken = default);
}
