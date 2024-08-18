using FluentValidation;

namespace Customer.API.Identity.Register;

public record RegisterRequest(string username, string email, string password, string firstName, string lastName);

public record RegisterResponse(bool success, string message);

public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/register", async (RegisterRequest request, ISender sender) =>
        {
            var command = new RegisterCommand(request.username, request.email, request.password, request.firstName, request.lastName);
            var result = await sender.Send(command);
            if(result.Succeeded)
            {
                return Results.Ok();
            }
            else
            {
                return Results.Problem(result.Message);
            }
        }).AllowAnonymous();
    }
}
