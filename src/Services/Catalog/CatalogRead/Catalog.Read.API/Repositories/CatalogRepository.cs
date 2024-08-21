using Marten.Pagination;

namespace Catalog.Read.API.Repositories;

public class CatalogRepository(IDocumentSession session) : ICatalogRepository
{
    public async Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        session.Store(product);
        await session.SaveChangesAsync(cancellationToken);

        return product;
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default)
    {
        session.Delete<Product>(id);
        await session.SaveChangesAsync(cancellationToken);
    }

    public async Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await session.LoadAsync<Product>(id, cancellationToken);

        return product;
    }

    public async Task<IPagedList<Product>> GetProductsByCategoryPagedAsync(string category, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var products = await session.Query<Product>()
            .Where(x => x.Categories.Contains(category))
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);

        return products;
    }

    public async Task<IPagedList<Product>> GetProductsPagedAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default)
    {
        var products = await session.Query<Product>()
            .ToPagedListAsync(pageNumber, pageSize, cancellationToken);

        return products;
    }

    public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return product;
    }

}
