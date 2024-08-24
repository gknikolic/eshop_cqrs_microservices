namespace Catalog.Write.Application.Repositories;
public interface ICategoryRepository
{
    Task<Category> GetAsync(Guid id);
    Task<Category> GetOrCreateAsync(string name);
    Task<IEnumerable<Category>> GetAllAsync();
}
