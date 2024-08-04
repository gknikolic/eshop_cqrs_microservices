using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using Inventory.API.Exceptions;
using Inventory.API.Repositories;
using MassTransit;
using MediatR;

namespace Inventory.API.CQRS.Commands.UpdateProductQuantity;

public class UpdateProductQuantityCommandHandler(IInventoryRepository repository, IPublishEndpoint publishEndpoint) 
    : ICommandHandler<UpdateProductQuantityCommand>
{
    public async Task<Unit> Handle(UpdateProductQuantityCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        product.Quantity += request.QuantityChangedBy;
        await repository.UpdateProductAsync(product, cancellationToken);

        await publishEndpoint.Publish(new ProductQuantityUpdatedEvent(product.Id, product.Quantity), cancellationToken);

        return Unit.Value;
    }
}
