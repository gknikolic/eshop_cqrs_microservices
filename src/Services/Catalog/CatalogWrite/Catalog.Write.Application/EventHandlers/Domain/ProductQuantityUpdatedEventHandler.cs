namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductQuantityUpdatedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ProductQuantityUpdatedEvent>
{
    public async Task Handle(ProductQuantityUpdatedEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new ProductQuantityUpdatedIntegrationEvent(notification.ProductId, notification.Quantity));
    }
}
