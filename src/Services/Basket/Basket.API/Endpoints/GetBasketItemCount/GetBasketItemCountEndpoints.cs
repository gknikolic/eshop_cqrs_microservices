namespace Basket.API.Endpoints.GetBasketItemCount;

public class GetBasketItemCountEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}/count", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketItemCountQuery(userName));

            return Results.Ok(result);
        });
    }
}
