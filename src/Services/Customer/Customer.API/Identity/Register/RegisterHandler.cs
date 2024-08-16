namespace Customer.API.Identity.Register;

public record RegisterCommand(string Username, string Email, string Password) : ICommand<Result>;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.Username).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class RegisterHandler(UserManager<User> _userManager)
    : ICommandHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = command.Username,
            Email = command.Email
        };

        var result = await _userManager.CreateAsync(user, command.Password);
        if (!result.Succeeded)
        {
            return Result.Failure(result.ToString());
        }

        // Optional: Assign "User" role by default to the new user
        await _userManager.AddToRoleAsync(user, "User");

        return Result.Success("User registrated");
    }
}
