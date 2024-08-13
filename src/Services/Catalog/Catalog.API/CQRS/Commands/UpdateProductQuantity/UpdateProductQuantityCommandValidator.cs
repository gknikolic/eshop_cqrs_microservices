namespace Catalog.API.CQRS.Commands.UpdateProductQuantity;

public class UpdateProductQuantityCommandValidator : AbstractValidator<UpdateProductQuantityCommand>
{
    public UpdateProductQuantityCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product id can't be empty.");
        RuleFor(x => x.Quantity).GreaterThanOrEqualTo(0).WithMessage("Quantity can't be negative.");
    }
}