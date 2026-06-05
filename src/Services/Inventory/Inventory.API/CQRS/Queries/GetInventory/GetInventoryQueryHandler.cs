using BuildingBlocks.CQRS;
using Inventory.API.Repositories;

namespace Inventory.API.CQRS.Queries.GetInventory;

public class GetInventoryQueryHandler(IInventoryRepository repository)
    : IQueryHandler<GetInventoryQuery, GetInventoryResult>
{
    public async Task<GetInventoryResult> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
    {
        var result = await repository.GetAllProductsAsync(cancellationToken);

        return new GetInventoryResult(result);
    }
}
