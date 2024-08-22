using Catalog.Write.Domain.Models;

namespace Catalog.Write.Domain.Events;
public record ProductCreatedEvent(Product Product) : IDomainEvent;

