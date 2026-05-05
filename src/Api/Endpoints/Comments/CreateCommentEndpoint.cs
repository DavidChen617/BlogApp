using Application.Comments.Commands;
using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Endpoints;
using CoreMesh.Validation.Abstractions;

namespace Api.Endpoints.Comments;

public sealed class CreateCommentEndpoint : IGroupedEndpoint<CommentsGroup>
{
    public void AddRoute(RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            CreateCommentCommand command,
            IDispatcher dispatcher,
            IValidator validator,
            CancellationToken ct) =>
        {
            var result = validator.Validate(command);
            if (!result.IsValid)
                return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Key, e => e.Value.ToArray()));

            await dispatcher.Send(command, ct);
            return Results.Created();
        });
    }
}
