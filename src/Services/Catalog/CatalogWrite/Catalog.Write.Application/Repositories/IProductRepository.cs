namespace Catalog.Write.Application.Repositories;
public interface IProductRepository
{
    Task<Product> GetAsync(Guid id);
    Task<IEnumerable<Product>> GetAllAsync(IEnumerable<Guid> ids);
}
