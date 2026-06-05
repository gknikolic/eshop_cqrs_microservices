
namespace Catalog.Read.API.Products.GetProductByCategory;

public record GetProductByCategoryRequest(string category, int? PageNumber = 1, int? PageSize = 10);
public record GetProductByCategoryResponse(IEnumerable<Product> Products);
public class GetProductByCategoryEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products-by-category",
            async ([AsParameters] GetProductByCategoryRequest request, ISender sender) =>
            {
                var result = await sender.Send(new GetProductByCategoryQuery(request.category, request.PageNumber, request.PageSize));

                var response = result.Adapt<GetProductByCategoryResponse>();

                return Results.Ok(response);
            })
        .WithName("GetProductByCategory")
        .Produces<GetProductByCategoryResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Product By Category")
        .WithDescription("Get Product By Category");
    }
}
