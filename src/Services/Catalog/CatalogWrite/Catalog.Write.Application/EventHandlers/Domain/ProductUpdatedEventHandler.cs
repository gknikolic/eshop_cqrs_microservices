
namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductUpdatedEventHandler
    : INotificationHandler<ProductUpdatedEvent>
{
    public Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
