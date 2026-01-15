
using Security.Application.Dtos.User;
using Security.Application.Users.Commands.CreateUser;

namespace Security.API.Endpoints.Users;

public record CreateUserRequest(UserDto User);
public record CreateUserResponde(int id);

public class CreateUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/user", async (CreateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateUserCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<CreateUserResponde>();
            return Results.Created($"users/{response.id}", response);
        })
        .WithName("Create User")
        .Produces<CreateUserResponde>(StatusCodes.Status201Created)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Create User")
        .WithDescription("Create User");
    }
}
