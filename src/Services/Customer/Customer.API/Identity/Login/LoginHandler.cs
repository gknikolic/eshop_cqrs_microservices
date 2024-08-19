using Customer.API.Services;

namespace Customer.API.Identity.Login;

public record LoginCommand(string email, string password) : ICommand<LoginResult>;
public record LoginResult(bool isLogedIn, string token = default!, string refreshToken = default!);

public class LoginCommadValidator : AbstractValidator<LoginCommand>
{
    public LoginCommadValidator()
    {
        RuleFor(x => x.email).NotEmpty().EmailAddress();
        RuleFor(x => x.password).NotEmpty();
    }
}

public class LoginHandler(IAuthService _authService)
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _authService.Login(new LoginUserModel(request.email, request.password));

        return result;
    }
}
