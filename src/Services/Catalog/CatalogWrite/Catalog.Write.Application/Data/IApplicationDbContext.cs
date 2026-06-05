namespace Catalog.Write.Application.Data;
public interface IApplicationDbContext
{
    DbSet<Product> Products { get; set; }
    DbSet<Category> Categories { get; set; }
    DbSet<ProductImage> ProductImages { get; set; }
    DbSet<ProductReview> ProductReviews { get; set; }
    DbSet<ProductStockChangeEvent> ProductStockChangeEvents { get; set; }
    DbSet<ProductAttribute> ProductAttributes { get; set; }
    DbSet<Customer> Customers { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
