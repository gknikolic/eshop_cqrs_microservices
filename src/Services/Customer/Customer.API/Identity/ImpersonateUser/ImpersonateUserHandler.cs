using Customer.API.Identity.Login;
using Customer.API.Services;

namespace Customer.API.Identity.ImpersonateUser;

public record ImpersonateUserQuery(string userId) : IQuery<LoginResult>;
public class ImpersonateUserHandler(IAuthService _authService) 
    : IQueryHandler<ImpersonateUserQuery, LoginResult>
{
    public async Task<LoginResult> Handle(ImpersonateUserQuery request, CancellationToken cancellationToken)
    {
        var result = await _authService.ImpersonateUser(request.userId);

        return result;
    }
}

