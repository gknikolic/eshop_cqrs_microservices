using BuildingBlocks.Messaging.Events.ProductEvents;

namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductDeletedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ProductDeletedEvent>
{
    public async Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new ProductDeletedIntegrationEvent(notification.ProductId));
    }
}
