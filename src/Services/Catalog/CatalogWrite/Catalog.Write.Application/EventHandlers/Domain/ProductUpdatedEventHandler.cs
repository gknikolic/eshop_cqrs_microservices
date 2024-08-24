
using BuildingBlocks.Messaging.Events.ProductEvents;
using MassTransit.Transports;

namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductUpdatedEventHandler(IPublishEndpoint publishEndpoint)
    : INotificationHandler<ProductUpdatedEvent>
{
    public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        var message = new ProductUpdatedIntegrationEvent();

        message.Id = notification.Product.Id;
        message.Name = notification.Product.Name;
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
