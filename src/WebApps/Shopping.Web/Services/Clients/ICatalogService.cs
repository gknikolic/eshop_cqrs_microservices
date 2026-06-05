using Refit;
using Shopping.Web.Models;

namespace Shopping.Web.Services.Clients;

public interface ICatalogService
{
    // Queries - only Get methods
    [Get("/catalog-service/products?pageNumber={pageNumber}&pageSize={pageSize}")]
    Task<GetProductsResponse> GetProducts(int? pageNumber = 1, int? pageSize = 10);

    [Get("/catalog-service/products/{id}")]
    Task<GetProductByIdResponse> GetProduct(Guid id);

    [Get("/catalog-service/products/category/{category}")]
    Task<GetProductByCategoryResponse> GetProductsByCategory(string category);

    // Commands - only Post, Put, Delete methods

    [Post("/catalog-service/products")]
    Task<ResultDto> CreateProduct(CreateProductRequest request);

    [Put("/catalog-service/products")]
    Task<ResultDto> UpdateProduct([Body]UpdateProductRequest request);

    [Delete("/catalog-service/products")]
    Task<ResultDto> DeleteProduct(Guid productId);

    [Post("/catalog-service/products/review")]
    Task<ResultDto> ReviewProduct(ReviewProductRequest request);
}
