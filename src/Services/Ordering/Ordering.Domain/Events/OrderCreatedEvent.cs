using BuildingBlocks.DDD_Abstractions;

namespace Ordering.Domain.Events;

public record OrderCreatedEvent(Order order) : IDomainEvent;
