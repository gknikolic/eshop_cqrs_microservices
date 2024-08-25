using NuGet.Configuration;

namespace Shopping.Web.Services;

public interface ITokenProvider
{
    void ClearTokens();
    string? GetToken();
    void SetToken(string token);

    string? GetRefreshToken();
    void StoreRefreshToken(string refreshToken);
}
