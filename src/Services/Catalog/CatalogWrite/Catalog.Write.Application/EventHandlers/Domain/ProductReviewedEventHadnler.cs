
using BuildingBlocks.Messaging.Events.ProductEvents;

namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductReviewedEventHadnler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ProductReviewedEvent>
{
    public async Task Handle(ProductReviewedEvent notification, CancellationToken cancellationToken)
    {
        var message = new ProductReviewedIntegrationEvent();
        message.ProductId = notification.ProductId;
        message.CustomerId = notification.Review.Customer.Id;
        message.CustomerName = notification.Review.Customer.Name;
        message.Comment = notification.Review.Comment;
        message.Rating = notification.Review.Rating;
        message.CreatedDate = notification.Review.CreatedAt ?? DateTime.UtcNow; // this date comes from save changes interceptor

        await publishEndpoint.Publish(message);
    }
}
