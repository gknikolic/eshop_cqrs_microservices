using Refit;
using Shopping.Web.Enums;
using Shopping.Web.Models;
using Shopping.Web.Models.Account;

namespace Shopping.Web.Services.Clients;

public interface ICustomerService
{
    [Post("/customer-service/login")]
    Task<LoginResponseDto> Login(LoginRequestDto request);

    [Post("/customer-service/register")]
    Task<string> Register(RegisterRequestDto request);

    [Get("/customer-service/users")]
    Task<GetCustomersResponse> GetCustomers();

    [Get("/customer-service/roles")]
    Task<IList<RoleEnum>> GetRoles();

    [Post("/customer-service/assignRole")]
    Task<ResultDto> ChangeUserRole(ChangeUserRoleRequest request);

    [Post("/customer-service/changePasswordForUser")]
    Task<ResultDto> ChangeUserPassword(ChangeUserPasswordDto request);

    [Get("/customer-service/impersonate")]
    Task<LoginResponseDto> ImpersonateUser(ImpersonateUserRequestDto request);
    
    [Get("/customer-service/deleteUser")]
    Task<ResultDto> DeleteUser(string userId);

    [Post("/customer-service/refreshToken")]
    Task<LoginResponseDto> RefreshToken(RefreshTokenRequestDto request);
}
