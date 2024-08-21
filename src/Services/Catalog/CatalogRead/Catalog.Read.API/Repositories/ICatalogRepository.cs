using Marten.Pagination;

namespace Catalog.Read.API.Repositories;

public interface ICatalogRepository
{
    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IPagedList<Product>> GetProductsPagedAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
    Task<IPagedList<Product>> GetProductsByCategoryPagedAsync(string category, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);
}
