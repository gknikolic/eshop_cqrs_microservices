namespace Catalog.Write.Domain.Events;
public class ProductQuantityUpdatedEvent(Guid ProductId, int Quantity) : IDomainEvent;
