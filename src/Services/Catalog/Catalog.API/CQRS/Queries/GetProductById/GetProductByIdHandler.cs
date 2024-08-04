using Catalog.API.Repositories;

namespace Catalog.API.CQRS.Queries.GetProductById;

internal class GetProductByIdQueryHandler
    (ICatalogRepository repository)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await repository.GetProductByIdAsync(query.Id, cancellationToken);

        if (product is null)
        {
            throw new ProductNotFoundException(query.Id);
        }

        return new GetProductByIdResult(product);
    }
}
