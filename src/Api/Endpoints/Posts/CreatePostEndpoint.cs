using Application.Posts.Commands;
using CoreMesh.Dispatching.Abstractions;
using CoreMesh.Endpoints;
using CoreMesh.Validation.Abstractions;

namespace Api.Endpoints.Posts;

public sealed class CreatePostEndpoint : IGroupedEndpoint<PostsGroup>
{
    public void AddRoute(RouteGroupBuilder group)
    {
        group.MapPost("/", async (
            CreatePostCommand command,
            ISender sender,
            IValidator validator,
            CancellationToken ct) =>
        {
            var result = validator.Validate(command);
            if (!result.IsValid)
                return Results.ValidationProblem(result.Errors.ToDictionary(e => e.Key, e => e.Value.ToArray()));

            await sender.Send(command, ct);
            return Results.Created();
        });
    }
}
