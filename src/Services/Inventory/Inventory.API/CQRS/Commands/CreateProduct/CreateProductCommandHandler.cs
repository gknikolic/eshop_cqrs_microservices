using BuildingBlocks.CQRS;
using Inventory.API.Repositories;
using MediatR;

namespace Inventory.API.CQRS.Commands.CreateProduct;

public class CreateProductCommandHandler(IInventoryRepository repository) 
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var result = await repository.AddProductAsync(request.Product);

        return new CreateProductResult(result.Id);
    }
}
