using Carter;
using Inventory.API.CQRS.Queries.GetInventory;
using Inventory.API.Models;
using Inventory.API.Repositories;
using Mapster;
using MediatR;

namespace Inventory.API.Endpoints;

public record GetInventoryResponse(IEnumerable<Product> Products);


public class GetInventoryEndpoints() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/inventory", async (ISender sender) =>
        {
            var result = await sender.Send(new GetInventoryQuery());

            var response = result.Products.Adapt<GetInventoryResponse>();

            return Results.Ok(result);
        })
        .WithName("GetInventory")
        .Produces<GetInventoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Category")
        .WithDescription("Get Product By Category"); ;
    }
}

