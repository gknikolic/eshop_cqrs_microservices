
namespace Catalog.Write.Application.EventHandlers.Domain;
public class ProductReviewdEventHadnler
    : INotificationHandler<ProductReviewedEvent>
{
    public Task Handle(ProductReviewedEvent notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
