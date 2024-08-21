namespace Catalog.Write.Domain.Events;
public class ProductDeletedEvent(Guid ProductId) : IDomainEvent;
