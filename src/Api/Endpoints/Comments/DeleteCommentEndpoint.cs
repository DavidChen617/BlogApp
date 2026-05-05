using Application.Comments.Commands;
using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Endpoints;

namespace Api.Endpoints.Comments;

public sealed class DeleteCommentEndpoint : IGroupedEndpoint<CommentsGroup>
{
    public void AddRoute(RouteGroupBuilder group)
    {
        group.MapDelete("/{id:guid}", async (
            Guid id,
            IDispatcher dispatcher,
            CancellationToken ct) =>
        {
            await dispatcher.Send(new DeleteCommentCommand(id), ct);
            return Results.NoContent();
        });
    }
}
