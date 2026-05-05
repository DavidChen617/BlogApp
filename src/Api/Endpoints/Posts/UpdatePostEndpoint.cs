using Application.Posts.Commands;
using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Endpoints;
using CoreMesh.Validation.Abstractions;

namespace Api.Endpoints.Posts;

public sealed record UpdatePostRequest(string Title, string Content);

public sealed class UpdatePostEndpoint : IGroupedEndpoint<PostsGroup>
{
    public void AddRoute(RouteGroupBuilder group)
    {
        group.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePostRequest body,
            IDispatcher dispatcher,
            IValidator validator,
            CancellationToken ct) =>
        {
            var command = new UpdatePostCommand(id, body.Title, body.Content);
            var result = validator.Validate(command);
            if (!result.IsValid)
                return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Key, e => e.Value.ToArray()));

            await dispatcher.Send(command, ct);
            return Results.NoContent();
        });
    }
}
