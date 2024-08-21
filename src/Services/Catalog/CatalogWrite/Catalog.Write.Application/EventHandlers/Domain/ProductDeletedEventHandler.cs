namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductDeletedEventHandler
    : INotificationHandler<ProductDeletedEvent>
{
    public Task Handle(ProductDeletedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
