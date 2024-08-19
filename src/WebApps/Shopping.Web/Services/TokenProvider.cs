﻿using Shopping.Web.Helpers;

namespace Shopping.Web.Services;

public class TokenProvider(IHttpContextAccessor httpContextAccessor)
    : ITokenProvider
{
    public void ClearToken()
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(Constants.AccessTokenCookieName);
        httpContextAccessor.HttpContext?.Response.Cookies.Delete(Constants.RefreshTokenCookieName);
    }

    public string? GetToken()
    {
        string? token = null;
        bool hasToken = httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(Constants.AccessTokenCookieName, out token) ?? false;

        return hasToken ? token : null;
    }

    public void SetToken(string token)
    {
        httpContextAccessor.HttpContext?.Response.Cookies.Append(Constants.AccessTokenCookieName, token);
    }

    public void StoreRefreshToken(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            Expires = DateTime.Now.AddDays(7) // Set refresh token expiration
        };

        httpContextAccessor.HttpContext.Response.Cookies.Append(Constants.RefreshTokenCookieName, refreshToken, cookieOptions);
    }
}
