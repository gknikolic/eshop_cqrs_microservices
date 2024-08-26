using BuildingBlocks.CQRS;
using Inventory.API.Dtos;
namespace Inventory.API.CQRS.Commands.UpdateProductQuantity;

public class UpdateInventoryItemCommandValidator : AbstractValidator<UpdateInventoryItemCommand>
{
    public UpdateInventoryItemCommandValidator()
    {
        RuleFor(x => x.ItemDto)
            .NotNull()
            .WithMessage("ItemDto can't be null.");

        When(x => x.ItemDto != null, () =>
        {
            RuleFor(x => x.ItemDto.Id)
                .NotEmpty()
                .WithMessage("Product id can't be empty.");
            RuleFor(x => x.ItemDto.Quantity)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Quantity can't be negative.");
        });
    }
}

public record UpdateInventoryItemCommand(InventoryItemDto ItemDto) : ICommand<UpdateInventoryItemCommandResponse>;
public record UpdateInventoryItemCommandResponse(InventoryItemDto ItemDto);
