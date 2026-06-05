using BuildingBlocks.Messaging.Events.InventoryEvents;
using Inventory.API.Exceptions;
using Inventory.API.Repositories;

namespace Inventory.API.CQRS.Commands.UpdateProductQuantityBy;

public class UpdateProductQuantityByCommandHandler
    (IInventoryRepository repository, ILogger<UpdateProductQuantityByCommandHandler> logger, IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateProductQuantityByCommand>
{
    public async Task<Unit> Handle(UpdateProductQuantityByCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        product.Quantity -= request.QuantityChangedBy;

        await repository.UpdateProductAsync(product, cancellationToken);

        logger.LogInformation("Product quantity updated: {ProductId}, {QuantityChangedBy}", product.Id, request.QuantityChangedBy);

        await publishEndpoint.Publish(new ProductQuantityUpdatedIntegrationEvent(product.Id, product.Quantity), cancellationToken);

        return Unit.Value;
    }
}
