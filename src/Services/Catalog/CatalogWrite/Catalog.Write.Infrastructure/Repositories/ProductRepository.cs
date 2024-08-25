using BuildingBlocks.Exceptions;
using Catalog.Write.Application.Data;
using Catalog.Write.Application.Repositories;
using Catalog.Write.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Write.Infrastructure.Repositories;
public class ProductRepository(IApplicationDbContext dbContext)
    : IProductRepository
{
    private IQueryable<Product> Products => dbContext.Products
        .AsNoTracking()
        //.Include(x => x.Category)
        //.Include(x => x.Reviews)
        //.Include(x => x.Images)
        .Include(x => x.Attributes);
        //.AsSplitQuery()
        //.AsNoTracking();

    public async Task<IEnumerable<Product>> GetAllAsync(IEnumerable<Guid> ids)
    {
        var products = await Products.Where(x => ids.Contains(x.Id)).ToListAsync();

        if (products != null && products.Any())
        {
            if (products.Count != ids.Count())
            {
                throw new NotFoundException(@$"Not all product found from order: orderItems={ids.Count()} found={products.Count}.
                Missing products with ids: {string.Join(", ", ids.Except(products.Select(x => x.Id.Value)))}");
            }
        }

        return products;
    }

    public async Task<Product> GetAsync(Guid id)
    {
        //var p = await Products.Where(x => x.Id == id).ToListAsync();
        //var product = p.FirstOrDefault(x => x.Id == id);
        var product = Products.FirstOrDefault(x => x.Id == id);

        if (product == null)
        {
            throw new Exception($"Product with id: {id} not found");
        }

        return product;
    }
}
