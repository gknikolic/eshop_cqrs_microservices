using BuildingBlocks.CQRS;

namespace Inventory.API.CQRS.Commands.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand;
