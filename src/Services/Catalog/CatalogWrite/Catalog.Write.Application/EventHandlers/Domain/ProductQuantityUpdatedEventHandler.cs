namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductQuantityUpdatedEventHandler
    : INotificationHandler<ProductQuantityUpdatedEvent>
{
    public Task Handle(ProductQuantityUpdatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
