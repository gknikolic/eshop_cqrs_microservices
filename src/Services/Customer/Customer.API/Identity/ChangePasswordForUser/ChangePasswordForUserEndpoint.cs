
namespace Customer.API.Identity.ChangePasswordForUser;

public record ChangePasswordRequest(string userId, string currentPassword, string newPassword);
public class ChangePasswordForUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/changePasswordForUser", async (ChangePasswordRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ChangePasswordForUserCommand(request.userId, request.currentPassword, request.newPassword));
            if (result.Succeeded)
            {
                return Results.Ok();
            }
            else
            {
                return Results.BadRequest(result.Message);
            }
        });
    }
}
