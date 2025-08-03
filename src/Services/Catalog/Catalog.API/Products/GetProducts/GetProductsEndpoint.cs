namespace Catalog.API.Products.GetProducts;

public record GetProductsReponse(IEnumerable<Product> Products);

public class GetProductsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProductsQuery());
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