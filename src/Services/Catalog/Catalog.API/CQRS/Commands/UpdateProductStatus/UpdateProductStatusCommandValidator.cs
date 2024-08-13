namespace Catalog.API.CQRS.Commands.UpdateProductStatus;

public class UpdateProductStatusCommandValidator : AbstractValidator<UpdateProductStatusCommand>
{
    public UpdateProductStatusCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product id can't be empty.");
    }
}
