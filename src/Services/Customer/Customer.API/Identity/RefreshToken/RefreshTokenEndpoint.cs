
namespace Customer.API.Identity.RefreshToken;

public record RefreshTokenRequest(string Token, string RefreshToken);
public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/refreshToken", async (RefreshTokenRequest request, ISender sender) =>
        {
            var result = await sender.Send(new RefreshTokenCommand(request.Token, request.RefreshToken));

            return Results.Ok(result);
        });
    }
}
