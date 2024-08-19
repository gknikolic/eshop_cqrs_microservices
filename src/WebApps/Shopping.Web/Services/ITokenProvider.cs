﻿using NuGet.Configuration;

namespace Shopping.Web.Services;

public interface ITokenProvider
{
    void ClearToken();
    string? GetToken();
    void SetToken(string token);

    void StoreRefreshToken(string refreshToken);
}
