
namespace Customer.API.Identity.ChangePassword;

public record ChangePasswordRequest(string currentPassword, string newPassword);

public class ChangePasswordEndpoint() : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/changePassword", async (ChangePasswordRequest request, ISender sender) =>
        {
            var result = await sender.Send(new ChangePasswordCommand(request.currentPassword, request.newPassword));
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
