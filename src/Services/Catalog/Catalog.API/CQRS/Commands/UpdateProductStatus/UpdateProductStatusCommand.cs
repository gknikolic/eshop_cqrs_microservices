namespace Catalog.API.CQRS.Commands.UpdateProductStatus;

public record UpdateProductStatusCommand(Guid Id, bool IsActive) : ICommand;
