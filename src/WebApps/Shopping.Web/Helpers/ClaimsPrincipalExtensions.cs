using Microsoft.AspNetCore.Authorization;
using Shopping.Web.Enums;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Shopping.Web.Helpers;

public static class ClaimsPrincipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(ClaimTypes.Name);
    }

    public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
    {
        return new Guid(claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Sub));
    }

    public static string? GetName(this ClaimsPrincipal claimsPrincipal)
    {
        return claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Name);
    }

    public static RoleEnum GetRole(this ClaimsPrincipal claimsPrincipal)
    {
        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);

        if(role == null)
        {
            return RoleEnum.User;
        }

        return Enum.Parse<RoleEnum>(role);
    }

    public static bool IsInRole(this ClaimsPrincipal claimsPrincipal, RoleEnum role)
    {
        return claimsPrincipal.GetRole() == role;
    }
}
