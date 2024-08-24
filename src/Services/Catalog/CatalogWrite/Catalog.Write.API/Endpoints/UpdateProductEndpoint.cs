namespace Catalog.Write.API.Endpoints;

public record UpdateProductRequest(ProductDto ProductModel);
public record UpdateProductResponse(bool IsUpdated, string Message);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (ISender sender, UpdateProductRequest request) =>
        {
            var result = await sender.Send(new UpdateProductCommand(request.ProductModel));
            return new UpdateProductResponse(result.IsUpdated, result.Message);
        });
    }
}
