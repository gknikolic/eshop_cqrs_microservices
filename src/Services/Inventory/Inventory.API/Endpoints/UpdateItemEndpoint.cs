using Carter;
using Inventory.API.CQRS.Commands.UpdateProductQuantity;
using Inventory.API.Dtos;

namespace Inventory.API.Endpoints;

public record UpdateItemRequest(InventoryItemDto ItemDto);

public class UpdateItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/inventory/update", async (ISender sender, UpdateItemRequest request) =>
        {
            await sender.Send(request.Adapt<UpdateInventoryItemCommand>());

            return Results.Ok();
        });
    }
}
