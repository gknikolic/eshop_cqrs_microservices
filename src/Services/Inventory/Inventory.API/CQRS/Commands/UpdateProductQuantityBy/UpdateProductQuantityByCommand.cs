namespace Inventory.API.CQRS.Commands.UpdateProductQuantityBy;

public record UpdateProductQuantityByCommand(Guid Id, int QuantityChangedBy) : ICommand;

public class UpdateProductQuantityByCommandValidator : AbstractValidator<UpdateProductQuantityByCommand>
{
    public UpdateProductQuantityByCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product id can't be empty.");
        RuleFor(x => x.QuantityChangedBy).GreaterThan(0).WithMessage("QuantityChangedBy must be greater then zero.");
    }
}
