using Catalog.Write.Application.Data;
using Catalog.Write.Application.Repositories;
using Catalog.Write.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Write.Infrastructure.Repositories;
public class CategoryRepository(IApplicationDbContext dbContext)
    : ICategoryRepository
{
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await dbContext.Categories.ToListAsync();
    }

    public async Task<Category> GetAsync(Guid id)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            throw new Exception($"Category with id: {id} not found");
        }
        return category;
    }

    public async Task<Category> GetOrCreateAsync(string name)
    {
        var category = await dbContext.Categories.FirstOrDefaultAsync(x => x.Name == name);
        if (category == null)
        {
            category = new Category(name, string.Empty);
            await dbContext.Categories.AddAsync(category);
        }
        return category;
    }
}
