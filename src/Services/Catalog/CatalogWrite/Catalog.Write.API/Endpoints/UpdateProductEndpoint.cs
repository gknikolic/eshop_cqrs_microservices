using Microsoft.AspNetCore.Mvc;

namespace Catalog.Write.API.Endpoints;

public record UpdateProductRequest(ProductDto product);
public record UpdateProductResponse(bool IsUpdated, string Message);

public class UpdateProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateProductCommand(request.product));
            return new UpdateProductResponse(result.IsUpdated, result.Message);
        });
    }
}
