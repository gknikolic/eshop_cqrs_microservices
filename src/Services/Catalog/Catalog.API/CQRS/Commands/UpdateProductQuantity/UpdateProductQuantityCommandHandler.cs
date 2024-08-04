using Catalog.API.Repositories;
using MassTransit;

namespace Catalog.API.CQRS.Commands.UpdateProductQuantity;

public class UpdateProductQuantityCommandHandler(ICatalogRepository repository, IPublishEndpoint publishEndpoint) 
    : ICommandHandler<UpdateProductQuantityCommand>
{
    public async Task<Unit> Handle(UpdateProductQuantityCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(request.Id);
        }

        product.PeicesInStock += request.QuantityChangedBy;
        await repository.UpdateProductAsync(product, cancellationToken);

        return Unit.Value;
    }
}
