
namespace Customer.API.Identity.Login;

public record LoginRequest(string email, string password);
public record LoginResponse(string token);

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login", async (LoginRequest request, ISender sender) =>
        {
            var command = new LoginCommand(request.email, request.password);

            var result = await sender.Send(command);

            if(result.success)
            {
                return Results.Ok(new LoginResponse(result.token));
            }
            else
            {
                return Results.Unauthorized();
            }
        }).AllowAnonymous();
    }
}
