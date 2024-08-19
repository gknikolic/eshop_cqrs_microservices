
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Shopping.Web.Helpers;
using Shopping.Web.Services.Clients;
using Shopping.Web.Models.Account;

namespace Shopping.Web.Services;

public class AuthService(IHttpContextAccessor _httpContextAccessor, ITokenProvider _tokenProvider, ICustomerService _customerService)
    : IAuthService
{

    private const int _TokenExpirationTime = 60;
    private const int _RefreshTokenExpirationTime = 7;

    public HttpContext HttpContext => _httpContextAccessor.HttpContext;

    public async Task SignInAsync(string token, string refreshToken, bool rememberMe = false)
    {
        var principal = CreateUserFromJwt(token);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = rememberMe,
            ExpiresUtc = rememberMe ? DateTimeOffset.UtcNow.AddDays(_RefreshTokenExpirationTime) : DateTimeOffset.UtcNow.AddMinutes(_TokenExpirationTime)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProperties);

        _tokenProvider.SetToken(token);
        _tokenProvider.StoreRefreshToken(refreshToken);
    }

    public async Task ValidateAndRefreshToken(CookieValidatePrincipalContext context)
    {
        // Extract the JWT token from the cookie
        //var identity = (ClaimsIdentity)context.Principal.Identity;
        //var accessToken = identity.FindFirst(Constants.AccessTokenCookieName)?.Value; // Ensure you include the JWT in the claims

        var accessToken = _tokenProvider.GetToken();

        if (string.IsNullOrEmpty(accessToken))
        {
            context.RejectPrincipal();
            return;
        }

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(accessToken);

        // Check if the JWT is about to expire
        var tokenExpiration = jwtToken.ValidTo;
        var timeToExpiration = tokenExpiration - DateTime.UtcNow;

        if (timeToExpiration > TimeSpan.FromMinutes(5))
        {
            // Token is still valid
            return;
        }

        // Token is close to expiring; refresh it
        var refreshToken = context.Request.Cookies[Constants.RefreshTokenCookieName];

        if (string.IsNullOrEmpty(refreshToken))
        {
            context.RejectPrincipal();
            return;
        }

        // Call the API to refresh the token
        var response = await _customerService.RefreshToken(new RefreshTokenRequestDto(accessToken, refreshToken));

        if (response.isLogedIn)
        {
            await SignInAsync(response.token!, response.refreshToken!, true);
        }
        else
        {
            // Refresh token is invalid or expired; sign out the user
            context.RejectPrincipal();
        }
    }

    private ClaimsPrincipal CreateUserFromJwt(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        var jwt = handler.ReadJwtToken(token);

        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)?.Value ?? string.Empty));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Name)?.Value ?? string.Empty));
        identity.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty));


        identity.AddClaim(new Claim(ClaimTypes.Name, jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Email)?.Value ?? string.Empty));
        identity.AddClaim(new Claim(ClaimTypes.Role, jwt.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value ?? string.Empty));

        var principal = new ClaimsPrincipal(identity);

        return principal;
    }
}
