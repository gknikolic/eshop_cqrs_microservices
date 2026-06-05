using BuildingBlocks.DDD_Abstractions;

namespace Ordering.Domain.Events;

public record OrderUpdatedEvent(Order order) : IDomainEvent;
