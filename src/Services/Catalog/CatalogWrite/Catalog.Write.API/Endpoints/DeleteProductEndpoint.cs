
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Write.API.Endpoints;
public record DeleteProductRequest(Guid ProductId, bool HardDelete = false);
public record DeleteProductResponse(bool IsDeleted, string Message);
public class DeleteProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/products", async ([FromBody]DeleteProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(new DeleteProductCommand(request.ProductId, request.HardDelete));
            return new DeleteProductResponse(result.IsDeleted, result.Message);
        });
    }
}
