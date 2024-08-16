
using BuildingBlocks.Authorization;
using Customer.API.Dtos;
using Microsoft.AspNetCore.Authorization;

namespace Customer.API.Identity.GetAllUsers;

public record GetUserRequest();
public record GetUsersResponse(List<UserDto> users);

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/users", async (ISender sender) =>
        {
            var result = await sender.Send(new GetUsersQuery());

            return Results.Ok(result);
        })
        .RequireAuthorization(RoleEnum.Admin)
        .WithMetadata(new AuthorizeAttribute { Roles = RoleEnum.Admin.ToString() })
        .WithTags("Users")
        .WithOpenApi(operation => new(operation)
        {
            Summary = "Get all users",
            Description = "Retrieves a list of all users. Requires Admin role."
        });
    }
}
