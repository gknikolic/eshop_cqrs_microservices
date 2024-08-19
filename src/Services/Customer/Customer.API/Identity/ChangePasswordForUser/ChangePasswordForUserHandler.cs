using Customer.API.Database.Entities;

namespace Customer.API.Identity.ChangePasswordForUser;

public record ChangePasswordForUserCommand(string userId, string currentPassword, string newPassword) : ICommand<Result>;

public class ChangePasswordForUserHandler(UserManager<User> userManager)
    : ICommandHandler<ChangePasswordForUserCommand, Result>
{
    public async Task<Result> Handle(ChangePasswordForUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.userId);

        if (user == null)
        {
            return Result.Failure("User not found");
        }

        var result = await userManager.ChangePasswordAsync(user, request.currentPassword, request.newPassword);
        if (!result.Succeeded)
        {
            return Result.Failure(result.ToString());
        }

        return Result.Success("Password changed.");
    }
}
