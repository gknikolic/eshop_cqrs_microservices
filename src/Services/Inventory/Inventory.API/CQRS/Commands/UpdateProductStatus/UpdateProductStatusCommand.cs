using BuildingBlocks.CQRS;

namespace Inventory.API.CQRS.Commands.UpdateProductStatus;

public record UpdateProductStatusCommand(Guid Id, bool IsActive) : ICommand;
