using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BuildingBlocks.Authorization;

public static class ClaimsPrintipalExtensions
{
    public static string? GetEmail(this ClaimsPrincipal claimsPrincipal)
    {
        if(claimsPrincipal == null)
        {
            return null;
        }
        return claimsPrincipal.FindFirstValue(ClaimTypes.Email);
    }

    public static Guid GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
        if(claimsPrincipal == null) {
            return Guid.Empty;
        }
        return new Guid(claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public static string? GetName(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal == null)
        {
            return null;
        }
        return claimsPrincipal.FindFirstValue(JwtRegisteredClaimNames.Name);
    }

    public static RoleEnum GetRole(this ClaimsPrincipal claimsPrincipal)
    {
        if (claimsPrincipal == null)
        {
            return RoleEnum.User;
        }

        var role = claimsPrincipal.FindFirstValue(ClaimTypes.Role);

        if (role == null)
        {
            return RoleEnum.User;
        }

        return Enum.Parse<RoleEnum>(role);
    }

    public static bool IsInRole(this ClaimsPrincipal claimsPrincipal, RoleEnum role)
    {
        if (claimsPrincipal == null)
        {
            return false;
        }
        return claimsPrincipal.GetRole() == role;
    }
}
