using Catalog.Write.Domain.Models;

namespace Catalog.Write.Domain.Events;
public class ProductCreatedEvent(Product Product) : IDomainEvent;

