
namespace Catalog.Write.API.Endpoints;
public record ReviewProductRequest(ProductReviewDto review);
public record ReviewProductResponse(bool IsReviewed, Guid? ProductReviewId);
public class ReviewProductEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/products/review", async (ReviewProductRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ReviewProductCommand(request.review));
            return new ReviewProductResponse(result.Success, result.ProductReviewId);
        }).RequireAuthorization();
    }
}
