using Customer.API.Database;

namespace Customer.API.Identity.DeleteUser;
public record DeleteUserCommand(string userId) : ICommand<Result>;
public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.userId).NotEmpty();
    }
}

public class DeleteUserHandler(UserManager<User> _userManager)
    : ICommandHandler<DeleteUserCommand, Result>
{

    public async Task<Result> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByIdAsync(command.userId);
        if (user == null)
            return Result.Failure("User not found!");

        var result = await _userManager.DeleteAsync(user);
        if (!result.Succeeded)
            return Result.Failure("Failed to delete user!");

        return Result.Success("User deleted successfully!");
    }
}
