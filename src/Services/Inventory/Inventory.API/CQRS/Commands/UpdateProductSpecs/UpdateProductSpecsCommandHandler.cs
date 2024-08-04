using BuildingBlocks.CQRS;
using Inventory.API.Exceptions;
using Inventory.API.Repositories;
using MediatR;

namespace Inventory.API.CQRS.Commands.UpdateProductSpecs;

public class UpdateProductSpecsCommandHandler(IInventoryRepository repository)
    : ICommandHandler<UpdateProductSpecsCommand>
{
    public async Task<Unit> Handle(UpdateProductSpecsCommand request, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(request.Product.Id, cancellationToken);
        if (product == null)
        {
            throw new ProductNotFoundException(request.Product.Id);
        }

        product.Name = request.Product.Name;
        product.Price = request.Product.Price;
        await repository.UpdateProductAsync(product, cancellationToken);

        return Unit.Value;
    }
}
