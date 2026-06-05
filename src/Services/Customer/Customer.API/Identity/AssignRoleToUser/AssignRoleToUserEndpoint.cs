
using BuildingBlocks.Authorization;

namespace Customer.API.Identity.AssignRoleToUser;

public record AssignRoleToUserRequest(string userId, IList<string> newRoles);

public class AssignRoleToUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/assignRole", async (AssignRoleToUserRequest request, ISender sender) =>
        {
            var command = new AssignRoleToUserCommand(request.userId, request.newRoles);

            var result = await sender.Send(command);

            return Results.Ok(result);
        }).RequireAuthorization(RoleEnum.Admin);
    }
}
