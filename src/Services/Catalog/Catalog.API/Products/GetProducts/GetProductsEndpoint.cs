namespace Catalog.API.Products.GetProducts;


public record GetProductsRquests(int? PageNumber = 1, int? PageSize = 10);

public record GetProductsReponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRquests request ,ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();
            var result = await sender.Send(query);
            var response = result.Adapt<GetProductsReponse>();
            return Results.Ok(response);
        })
            .WithName("GetProducts")
            .Produces<GetProductsReponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get products")
            .WithDescription("Get Products");
    }
}