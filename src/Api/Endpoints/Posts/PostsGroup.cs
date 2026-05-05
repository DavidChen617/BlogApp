using CoreMesh.Endpoints;

namespace Api.Endpoints.Posts;

public sealed class PostsGroup : IGroupEndpoint
{
    public string GroupPrefix => "/api/posts";

    public void Configure(RouteGroupBuilder group)
    {
        group.WithTags("Posts");
    }
}
