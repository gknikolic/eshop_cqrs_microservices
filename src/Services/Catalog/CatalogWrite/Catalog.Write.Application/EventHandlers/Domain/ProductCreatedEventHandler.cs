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
        message.Sku = notification.Product.Sku;
        message.Description = notification.Product.Description;
        message.Price = notification.Product.Price;
        message.Categories = new List<string> { notification.Product.Category.Name };
        message.ImageFiles = notification.Product.Images.OrderBy(x => x.DisplayOrder).Select(x => x.FilePath).ToList();
        message.Quantity = notification.Product.Stock.Quantity;
        message.Color = notification.Product.Color.ToString();
        message.ProductAttributes = notification
            .Product.Attributes
            .Select(x => new BuildingBlocks.Messaging.Events.ProductEvents.Abstraction.ProductAttribute { Id = x.Id, Name = x.Key, Value = x.Value })
            .ToList();

        await publishEndpoint.Publish(message, cancellationToken);
    }
}
