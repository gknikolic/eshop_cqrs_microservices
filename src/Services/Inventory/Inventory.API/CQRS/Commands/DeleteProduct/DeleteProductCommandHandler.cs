using BuildingBlocks.CQRS;
using Inventory.API.Repositories;
using MediatR;

namespace Inventory.API.CQRS.Commands.DeleteProduct;

public class DeleteProductCommandHandler(IInventoryRepository repository)
    : ICommandHandler<DeleteProductCommand>
{
    public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        await repository.DeleteProductAsync(request.Id, cancellationToken);

        return Unit.Value;
    }
}
