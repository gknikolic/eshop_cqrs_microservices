using Catalog.Read.API.Repositories;

namespace Catalog.Read.API.Products.GetProductByCategory;
public record GetProductByCategoryQuery(string Category, int? PageNumber = 1, int? PageSize = 10) : IQuery<GetProductByCategoryResult>;
public record GetProductByCategoryResult(IEnumerable<Product> Products);

internal class GetProductByCategoryHandler(ICatalogRepository repository)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategoryResult>
{
    public async Task<GetProductByCategoryResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await repository.GetProductsByCategoryPagedAsync(query.Category, query.PageNumber.Value, query.PageSize.Value, cancellationToken);

        return new GetProductByCategoryResult(products);
    }
}