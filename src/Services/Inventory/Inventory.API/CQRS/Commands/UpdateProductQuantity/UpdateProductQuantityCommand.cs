using BuildingBlocks.CQRS;

namespace Inventory.API.CQRS.Commands.UpdateProductQuantity;

public record UpdateProductQuantityCommand(Guid Id, int QuantityChangedBy) : ICommand;
