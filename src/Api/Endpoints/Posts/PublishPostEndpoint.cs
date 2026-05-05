using Application.Posts.Commands;
using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Endpoints;

namespace Api.Endpoints.Posts;

public sealed class PublishPostEndpoint : IGroupedEndpoint<PostsGroup>
{
    public void AddRoute(RouteGroupBuilder group)
    {
        group.MapPost("/{id:guid}/publish", async (
            Guid id,
            IDispatcher dispatcher,
            CancellationToken ct) =>
        {
            await dispatcher.Send(new PublishPostCommand(id), ct);
            return Results.NoContent();
        });
    }
}
