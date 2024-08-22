using BuildingBlocks.DDD_Abstractions;
using Catalog.Write.Domain.Models;

namespace Catalog.Write.Domain.Events;
public record ProductUpdatedEvent(Product Product) : IDomainEvent;
