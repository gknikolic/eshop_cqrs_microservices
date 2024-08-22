namespace Catalog.Write.Domain.Events;
public record ProductQuantityUpdatedEvent(Guid ProductId, int Quantity) : IDomainEvent;
