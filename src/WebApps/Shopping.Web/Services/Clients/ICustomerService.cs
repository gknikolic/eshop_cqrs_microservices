using Refit;
using Shopping.Web.Dtos.Account;

namespace Shopping.Web.Services.Clients;

public interface ICustomerService
{
    [Post("/customer-service/login")]
    Task<LoginResponseDto> Login(LoginRequestDto request);

    [Post("/customer-service/register")]
    Task<string> Register(RegisterRequestDto request);
}
