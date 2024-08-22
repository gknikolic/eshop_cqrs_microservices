using BuildingBlocks.Messaging.Events.ProductEvents;

namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductCreatedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ProductCreatedEvent>
{
    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        var message = new ProductCreatedIntegrationEvent();

        message.Id = notification.Product.Id;
        message.Name = notification.Product.Name;
        message.Description = notification.Product.Description;
        message.Price = notification.Product.Price;
        message.Categories = new List<string> { notification.Product.Category.Name };
        message.ImagePaths = notification.Product.Images.OrderBy(x => x.DisplayOrder).Select(x => x.FilePath).ToList();
        message.PeicesInStock = notification.Product.Stock.Quantity;
        message.IsActive = notification.Product.IsActive;

        await publishEndpoint.Publish(message, cancellationToken);
    }
}
