using FluentValidation;

namespace Ordering.Application.CQRS.Commands.CreateCustomer;
public record CreateCustomerCommand(CustomerDto CustomerDto) : ICommand<bool>;

public class CreateCustomerValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerValidator()
    {
        RuleFor(x => x.CustomerDto).NotNull();
        RuleFor(x => x.CustomerDto.Name).NotEmpty();
        RuleFor(x => x.CustomerDto.Email).NotEmpty().EmailAddress();
    }
}
