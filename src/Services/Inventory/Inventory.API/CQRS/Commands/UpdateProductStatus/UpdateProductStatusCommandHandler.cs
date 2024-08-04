using BuildingBlocks.CQRS;
using BuildingBlocks.Messaging.Events;
using Inventory.API.Exceptions;
using Inventory.API.Repositories;
using MassTransit;
using MediatR;

namespace Inventory.API.CQRS.Commands.UpdateProductStatus;

public class UpdateProductStatusCommandHandler(IInventoryRepository repository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<UpdateProductStatusCommand>
{
    public async Task<Unit> Handle(UpdateProductStatusCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        product.IsActive = request.IsActive;
        await repository.UpdateProductAsync(product, cancellationToken);

        await publishEndpoint.Publish(new ProductStatusUpdatedEvent(product.Id, product.IsActive), cancellationToken);

        return Unit.Value;
    }
}
