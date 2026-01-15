using Security.Application.Users.Commands.DeleteUser;

namespace Security.API.Endpoints.Users;

public record DeleteUserRequest(int UserId);
public record DeleteUserResponse(bool Success);

public class DeleteUser : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/user/{userId:int}", async (int userId, ISender sender) =>
        {
            var command = new DeleteUserCommand(userId);
            var result = await sender.Send(command);
            var response = result.Adapt<DeleteUserResponse>();
            return Results.Ok(response);
        })
        .WithName("Delete User")
        .Produces<DeleteUserResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .WithSummary("Delete User")
        .WithDescription("Delete User (Logical Delete)");
    }
}
