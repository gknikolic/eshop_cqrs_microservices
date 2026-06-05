namespace Catalog.Write.Domain.Events;
public record ProductDeletedEvent(Guid ProductId) : IDomainEvent;
