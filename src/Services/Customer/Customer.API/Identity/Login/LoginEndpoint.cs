
namespace Customer.API.Identity.Login;

public record LoginRequest(string email, string password);
public record LoginResponse(bool isLogedIn, string token, string refreshToken);

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (LoginRequest request, ISender sender) =>
        {
            var command = new LoginCommand(request.email, request.password);

            var result = await sender.Send(command);

            if(result.isLogedIn)
            {
                return Results.Ok(new LoginResponse(true, result.token, result.refreshToken));
            }
            else
            {
                return Results.Unauthorized();
            }
        }).AllowAnonymous();
    }
}
