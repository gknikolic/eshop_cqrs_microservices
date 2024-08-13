using BuildingBlocks.CQRS;
using FluentValidation;
using Inventory.API.Dtos;

namespace Inventory.API.CQRS.Commands.CreateProduct;

public record CreateProductCommand(ProductDto Product) : ICommand<CreateProductResult>;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Product).NotNull();
        RuleFor(x => x.Product.Id).NotEmpty();
        RuleFor(x => x.Product.Name).NotEmpty();
        RuleFor(x => x.Product.Price).GreaterThan(0);
    }
}

public record CreateProductResult(Guid Id);
