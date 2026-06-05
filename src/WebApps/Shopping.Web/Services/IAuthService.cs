using Microsoft.AspNetCore.Authentication.Cookies;

namespace Shopping.Web.Services;

public interface IAuthService
{
    Task SignInAsync(string token, string refreshToken, bool rememberMe = false);
    Task ValidateAndRefreshToken(CookieValidatePrincipalContext context);
}
