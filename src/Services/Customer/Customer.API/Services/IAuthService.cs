using Customer.API.Database.Entities;
using Customer.API.Identity.Login;

namespace Customer.API.Services;

public interface IAuthService
{
    Task<RegisterUserResult> Register(RegisterUserModel model);
    Task<LoginResult> Login(LoginUserModel credentials);

    Task<LoginResult> RefreshToken(string token, string refreshToken);
    Task<LoginResult> ImpersonateUser(string userId);
}
