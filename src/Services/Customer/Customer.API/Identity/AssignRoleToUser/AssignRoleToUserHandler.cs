using Customer.API.Database.Entities;

namespace Customer.API.Identity.AssignRoleToUser;

public record AssignRoleToUserCommand(string UserId, IList<string> NewRoles) : ICommand<Result>;

public class AssignRoleToUserValidator : AbstractValidator<AssignRoleToUserCommand>
{
    public AssignRoleToUserValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.NewRoles).NotEmpty();
    }
}

public class AssignRoleToUserHandler(UserManager<User> userManager)
    : ICommandHandler<AssignRoleToUserCommand, Result>
{
    public async Task<Result> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        // Find the user by ID
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null)
        {
            return Result.Failure("User not found");
        }

        // Get the current roles of the user
        var currentRoles = await userManager.GetRolesAsync(user);

        // Calculate roles to add and remove
        var rolesToAdd = request.NewRoles.Except(currentRoles).ToList();
        var rolesToRemove = currentRoles.Except(request.NewRoles).ToList();

        // Remove roles that are no longer needed
        if (rolesToRemove.Any())
        {
            var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!removeResult.Succeeded)
            {
                return Result.Failure("Failed to remove roles");
            }
        }

        // Add new roles
        if (rolesToAdd.Any())
        {
            var addResult = await userManager.AddToRolesAsync(user, rolesToAdd);
            if (!addResult.Succeeded)
            {
                return Result.Failure("Failed to add roles");
            }
        }

        return Result.Success("Roles updated successfully");
    }
}
