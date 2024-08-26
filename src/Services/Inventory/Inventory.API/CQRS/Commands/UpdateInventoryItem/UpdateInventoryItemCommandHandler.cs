using BuildingBlocks.Messaging.Events.InventoryEvents;
using Inventory.API.Exceptions;
using Inventory.API.Repositories;

namespace Inventory.API.CQRS.Commands.UpdateProductQuantity;

public class UpdateInventoryItemCommandHandler(IInventoryRepository repository, IPublishEndpoint publishEndpoint, ILogger<UpdateInventoryItemCommandHandler> logger) 
    : ICommandHandler<UpdateInventoryItemCommand, UpdateInventoryItemCommandResponse>
{
    public async Task<UpdateInventoryItemCommandResponse> Handle(UpdateInventoryItemCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.ItemDto.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(request.ItemDto.Id);
        }

        // check if the quantity is the same
        if (product.Quantity == request.ItemDto.Quantity)
        {
            return new UpdateInventoryItemCommandResponse(product.ToInventoryItemDto());
        }

        // update the quantity
        product.Quantity = request.ItemDto.Quantity;
        var updatedProduct = await repository.UpdateProductAsync(product, cancellationToken);

        // publish the event
        await publishEndpoint.Publish(new ProductQuantityUpdatedIntegrationEvent(product.Id, product.Quantity), cancellationToken);

        return new UpdateInventoryItemCommandResponse(updatedProduct.ToInventoryItemDto());
    }
}
