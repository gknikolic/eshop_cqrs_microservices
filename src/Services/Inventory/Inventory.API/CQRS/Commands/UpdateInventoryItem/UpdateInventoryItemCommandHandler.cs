﻿using BuildingBlocks.Messaging.Events.InventoryEvents;
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

        var oldQuantity = product.Quantity;
        var oldIsActive = product.IsActive;

        product.Quantity = request.ItemDto.Quantity;
        product.IsActive = request.ItemDto.IsActive;

        var updatedProduct = await repository.UpdateProductAsync(product, cancellationToken);

        logger.LogInformation("Product updated: {ProductId}, {Quantity}, {IsActive}", product.Id, request.ItemDto.Quantity, request.ItemDto.IsActive);

        if(oldQuantity != product.Quantity)
        {
            logger.LogInformation("Publishing {ProductQuantityUpdatedEvent}: {ProductId}, {QuantityChangedBy}", nameof(ProductQuantityUpdatedIntegrationEvent), product.Id, request.ItemDto.Quantity - oldQuantity);
            await publishEndpoint.Publish(new ProductQuantityUpdatedIntegrationEvent(product.Id, product.Quantity), cancellationToken);
        }

        if(oldIsActive != product.IsActive)
        {
            logger.LogInformation("Publishing {ProductStatusUpdatedEvent}: {ProductId}, {IsActive}", nameof(ProductStatusUpdatedIntegrationEvent), product.Id, product.IsActive);
            await publishEndpoint.Publish(new ProductStatusUpdatedIntegrationEvent(product.Id, product.IsActive), cancellationToken);
        }

        return new UpdateInventoryItemCommandResponse(updatedProduct.ToInventoryItemDto());
    }
}
