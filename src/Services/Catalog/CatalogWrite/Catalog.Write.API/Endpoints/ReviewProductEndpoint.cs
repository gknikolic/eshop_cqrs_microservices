
namespace Catalog.Write.API.Endpoints;
public record ReviewProductRequest(ProductReviewDto ProductReviewDto);
public record ReviewProductResponse(bool IsReviewed, Guid? ProductReviewId);
public class ReviewProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products/review", async (ISender sender, ReviewProductRequest request) =>
        {
            var result = await sender.Send(new ReviewProductCommand(request.ProductReviewDto));
            return new ReviewProductResponse(result.Success, result.ProductReviewId);
        });
    }
}
