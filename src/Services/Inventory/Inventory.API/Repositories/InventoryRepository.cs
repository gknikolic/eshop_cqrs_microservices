using Inventory.API.Dtos;
using Inventory.API.Models;
using Marten;

namespace Inventory.API.Repositories;

public class InventoryRepository(IDocumentSession session)
    : IInventoryRepository
{
    public async Task<Product> AddProductAsync(ProductDto product, CancellationToken cancellationToken = default)
    {
        var newProduct = new Product
        {
            Id = product.Id,
            Name = product.Name,
            Quantity = 0,
        };

        session.Store(newProduct);
        await session.SaveChangesAsync(cancellationToken);

        return newProduct;
    }

    public async Task DeleteProductAsync(Guid id, CancellationToken cancellationToken)
    {
        session.Delete<Product>(id);
        await session.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Product>> GetAllProductsAsync(CancellationToken cancellationToken = default)
    {
        var inventory = await session.Query<Product>().ToListAsync();

        return inventory.ToList();
    }

    public async Task<Product?> GetProductByIdAsync(Guid Id, CancellationToken cancellationToken = default)
    {
        var product = await session.LoadAsync<Product>(Id, cancellationToken);
        return product;
    }

    public async Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default)
    {
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);

        return product;
    }
}
