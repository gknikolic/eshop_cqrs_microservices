namespace Catalog.Write.API.Endpoints;

public record  CreateProductRequest(ProductDto ProductModel);
public record  CreateProductResponse(bool Success, Guid? ProductId);

public class CreateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(new CreateProductCommand(request.ProductModel));
            return new CreateProductResponse(result.Success, result.ProductId);
        });
    }
}
