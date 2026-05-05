using Application.Posts.Queries;
using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Endpoints;

namespace Api.Endpoints.Posts;

public sealed class GetPostEndpoint : IGroupedEndpoint<PostsGroup>
{
    public void AddRoute(RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (
            Guid id,
            ISender sender,
            CancellationToken ct) =>
        {
            var post = await sender.Send(new GetPostByIdQuery(id), ct);
            return Results.Ok(post);
        });
    }
}
