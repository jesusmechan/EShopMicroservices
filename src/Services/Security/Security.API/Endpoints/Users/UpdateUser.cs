using Security.Application.Dtos.User;
using Security.Application.Users.Commands.UpdateUser;

namespace Security.API.Endpoints.Users;

public record UpdateUserRequest(UserDto User);
public record UpdateUserResponse(bool Success);

public class UpdateUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/user", async (UpdateUserRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateUserCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<UpdateUserResponse>();
            return Results.Ok(response);
        })
        .WithName("Update User")
        .Produces<UpdateUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Update User")
        .WithDescription("Update User");
    }
}
