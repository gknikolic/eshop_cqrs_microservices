using Catalog.Write.Domain.Models;

namespace Catalog.Write.Domain.Events;
public record ProductReviewedEvent(Guid ProductId, ProductReview Review) : IDomainEvent;
