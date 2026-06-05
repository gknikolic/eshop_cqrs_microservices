using BuildingBlocks.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Identity.DeleteUser;
public record DeleteUserRequest(string userId);
public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/deleteUser", async ([FromBody]DeleteUserRequest request, ISender sender) =>
        {
            var result = await sender.Send(new DeleteUserCommand(request.userId));

            if(result.Succeeded)
            {
                return Results.Ok(result.Message);
            }
            else
            {
                return Results.BadRequest(result.Message);
            }
        }).RequireAuthorization(RoleEnum.Admin);
    }
}
