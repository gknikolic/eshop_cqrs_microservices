using Shopping.Web.Helpers;

namespace Shopping.Web.Services;

public class TokenProvider(IHttpContextAccessor httpContextAccessor)
    : ITokenProvider
{
    public void ClearToken()
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(Constants.TokenCookieName);
    }

    public string? GetToken()
    {
        string? token = null;
        bool hasToken = httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(Constants.TokenCookieName, out token) ?? false;

        return hasToken ? token : null;
    }

    public void SetToken(string token)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Append(Constants.TokenCookieName, token);
    }
}
