using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;

namespace BuildingBlocks.Authorization;
public static class Extensions
{
    public static IEndpointConventionBuilder RequireAuthorization(this IEndpointConventionBuilder builder, params RoleEnum[] roles)
    {
        if (roles == null || roles.Length == 0)
        {
            throw new ArgumentException("At least one role must be provided", nameof(roles));
        }

        // Convert enum values to role names
        var roleNames = Enum.GetNames(typeof(RoleEnum));

        var requiredRoles = string.Join(",", roles);

        // Add the authorization policy to the endpoint
        builder.RequireAuthorization(new AuthorizationPolicyBuilder()
            .RequireRole(roleNames)
            .Build());

        return builder;
    }
}
