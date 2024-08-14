using Carter;
using Inventory.API.CQRS.Commands.UpdateProductQuantity;
using Inventory.API.Dtos;

namespace Inventory.API.Endpoints;

public record UpdateItemRequest(InventoryItemDto InventoryItemDto);
public record UpdateItemResponse(InventoryItemDto InventoryItemDto);

public class UpdateItemEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/inventory", async (ISender sender, UpdateItemRequest request) =>
        {
            var result = await sender.Send(new UpdateInventoryItemCommand(request.InventoryItemDto));

            return Results.Ok(result.Adapt<UpdateItemResponse>());
        });
    }
}
