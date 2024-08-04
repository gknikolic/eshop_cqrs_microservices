using Catalog.API.Repositories;

namespace Catalog.API.CQRS.Queries.GetProducts;

internal class GetProductsQueryHandler
    (ICatalogRepository repository)
    : IQueryHandler<GetProductsQuery, GetProductsResult>
{
    public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await repository.GetProductsPagedAsync(query.PageNumber ?? 1, query.PageSize ?? 10, cancellationToken);

        return new GetProductsResult(products);
    }
}
