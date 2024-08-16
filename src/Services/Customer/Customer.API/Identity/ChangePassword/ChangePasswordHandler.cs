using System.Security.Claims;

namespace Customer.API.Identity.ChangePassword;

public record ChangePasswordCommand(string CurrentPassword, string NewPassword) : ICommand<Result>;

public class ChangePasswordValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword).NotEmpty();
        RuleFor(x => x.NewPassword).NotEmpty();
    }
}

public class ChangePasswordHandler(UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
    : ICommandHandler<ChangePasswordCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return Result.Failure("User not found");
        }

        var result = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        if (!result.Succeeded)
        {
            return Result.Failure(result.ToString());
        }

        return Result.Success("Password changed.");
    }
}
