namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductCreatedEventHandler
    : INotificationHandler<ProductCreatedEvent>
{
    public Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
