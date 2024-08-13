using BuildingBlocks.CQRS;
using FluentValidation;

namespace Inventory.API.CQRS.Commands.DeleteProduct;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Product id can't be empty.");
    }
}

public record DeleteProductCommand(Guid Id) : ICommand;
