using FluentValidation;

namespace Ordering.Application.CQRS.Commands.CreateProduct;

public record CreateProductCommand(Guid Id, string Name, decimal Price)
    : ICommand<CreateProductResult>;

public record CreateProductResult();

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Price).NotNull().WithMessage("Price is required");
    }
}