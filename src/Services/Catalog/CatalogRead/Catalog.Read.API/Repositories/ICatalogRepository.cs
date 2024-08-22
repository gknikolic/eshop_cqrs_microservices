using Marten.Pagination;

namespace Catalog.Read.API.Repositories;

public interface ICatalogRepository
{
    /// <summary>
    /// Adds a new product asynchronously.
    /// </summary>
    /// <param name="product">The product to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The added product.</returns>
    Task<Product> AddProductAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a product by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the product to retrieve.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The retrieved product, or null if not found.</returns>
    Task<Product?> GetProductByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing product asynchronously.
    /// </summary>
    /// <param name="product">The product to update.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated product.</returns>
    Task<Product> UpdateProductAsync(Product product, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a product by its ID asynchronously.
    /// </summary>
    /// <param name="id">The ID of the product to delete.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task DeleteProductAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paged list of products asynchronously.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of products per page (default is 10).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged list of products.</returns>
    Task<IPagedList<Product>> GetProductsPagedAsync(int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a paged list of products by category asynchronously.
    /// </summary>
    /// <param name="category">The category of the products to retrieve.</param>
    /// <param name="pageNumber">The page number to retrieve (default is 1).</param>
    /// <param name="pageSize">The number of products per page (default is 10).</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A paged list of products.</returns>
    Task<IPagedList<Product>> GetProductsByCategoryPagedAsync(string category, int pageNumber = 1, int pageSize = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a product review asynchronously.
    /// </summary>
    /// <param name="productId">The ID of the product to add the review to.</param>
    /// <param name="review">The product review to add.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    Task AddProductReviewAsync(Guid productId, ProductReview review, CancellationToken cancellationToken = default);
}
