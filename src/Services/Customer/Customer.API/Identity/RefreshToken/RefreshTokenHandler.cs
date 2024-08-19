using Customer.API.Identity.Login;
using Customer.API.Services;

namespace Customer.API.Identity.RefreshToken;

public record class RefreshTokenCommand(string token, string refreshToken) : ICommand<LoginResult>;

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.token).NotEmpty();
        RuleFor(x => x.refreshToken).NotEmpty();
    }
}

public class RefreshTokenHandler(IAuthService _authService)
    : ICommandHandler<RefreshTokenCommand, LoginResult>
{
    public async Task<LoginResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var reuslt = await _authService.RefreshToken(request.token, request.refreshToken);

        return reuslt;
    }
}
