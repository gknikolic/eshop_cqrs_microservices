
using BuildingBlocks.Authorization;
using Customer.API.Identity.ImpersonateUser;

namespace Customer.API.Identity.Impersonate;

public record ImpersonateUserRequest(string userId);

public class ImpersonateUserEndpoint : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/impersonate", async (ISender sender, ImpersonateUserRequest request) =>
        {
            var result = await sender.Send(new ImpersonateUserQuery(request.userId));

            return Results.Ok(result);
        }).RequireAuthorization(RoleEnum.Admin);
    }
}
