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

    [Post("/catalog-service/create-product")]
    Task<ResultDto> CreateProduct(CreateProductRequest request);

    [Put("/catalog-service/update-product")]
    Task<ResultDto> UpdateProduct(UpdateProductRequest request);

    [Delete("/catalog-service/update-product")]
    Task<ResultDto> DeleteProduct(Guid productId);

    [Post("/catalog-service/review-product")]
    Task<ResultDto> ReviewProduct(ReviewProductRequest request);
}
