using MassTransit;
using BuildingBlocks.Messaging.Events.CustomerEvents;
using Customer.API.Database.Entities;
using Customer.API.Services;

namespace Customer.API.Identity.Register;

public record RegisterCommand(RegisterUserModel RegisterUserModel) : ICommand<Result>;

public class RegisterValidator : AbstractValidator<RegisterCommand>
{
    public RegisterValidator()
    {
        RuleFor(x => x.RegisterUserModel.Username).NotEmpty();
        RuleFor(x => x.RegisterUserModel.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.RegisterUserModel.Password).NotEmpty();
        RuleFor(x => x.RegisterUserModel.FirstName).NotEmpty();
        RuleFor(x => x.RegisterUserModel.LastName).NotEmpty();
    }
}

public class RegisterHandler(IPublishEndpoint _publishEndpoint, IAuthService _authService, ILogger<RegisterHandler> _logger)
    : ICommandHandler<RegisterCommand, Result>
{
    public async Task<Result> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        
        var result = await _authService.Register(command.RegisterUserModel);

        if(!result.IsRegistered)
        {
            _logger.LogError(result.Message);
            return Result.Failure(result.Message);
        }

        var user = result.User;

        await _publishEndpoint.Publish(new UserRegisteredEvent(new Guid(user.Id), user.FullName, user.Email, "SelfRegistered"), cancellationToken);

        return Result.Success("User registrated");
    }
}
