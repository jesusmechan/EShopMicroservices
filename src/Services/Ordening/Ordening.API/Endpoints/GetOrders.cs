using BuildingBlocks.Pagination;
using Ordening.Application.Orders.Queries.GetOrders;

//public record GetOrderWithPaginationRequest(PaginationRequest request);
public record GetOrderWithPaginationResponse(PaginationResult<OrderDto> Orders);

namespace Ordening.API.Endpoints
{
    public class GetOrders : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/orders", async ([AsParameters] PaginationRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetOrdersQuery(request));
                var response = result.Adapt<GetOrderWithPaginationResponse>();
                return Results.Ok(response);

            })
            .WithName("GetOrdersWithPagination")
            .Produces<GetOrderWithPaginationResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Orders With Pagination")
            .WithDescription("Get Orders With Pagination");
        }
    }
}
