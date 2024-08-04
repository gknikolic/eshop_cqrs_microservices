using Inventory.API.Dtos;
using Inventory.API.Models;

namespace Inventory.API.Repositories;

public interface IInventoryRepository
{
    Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default);
    Task<Product> AddProductAsync(ProductDto product, CancellationToken cancellationToken = default);
    Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);
    Task<Product?> GetProductByIdAsync(Guid Id, CancellationToken cancellationToken = default);
    Task DeleteProductAsync(Guid id, CancellationToken cancellationToken);
}
