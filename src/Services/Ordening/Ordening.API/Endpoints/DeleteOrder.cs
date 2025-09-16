
//public record DeleteOrderRequest(Guid id);
using Ordening.Application.Orders.Commands.DeleteOrder;

public record DeleteOrderResponse(bool IsSuccess);

namespace Ordening.API.Endpoints
{
    public class DeleteOrder : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("/order/{id}", async (Guid Id, ISender sender) =>
            {
                var result = await sender.Send(new DeleteOrderCommand(Id));
                var response = result.Adapt<DeleteOrderResponse>();
                return Results.Ok(response);
            })
            .WithName("Delete Order")
            .Produces<CreateOrderResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Delete Order")
            .WithDescription("Delete Order");
        }
    }
}
