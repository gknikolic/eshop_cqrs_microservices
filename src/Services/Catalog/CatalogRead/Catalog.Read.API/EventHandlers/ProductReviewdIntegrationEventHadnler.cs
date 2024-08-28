using BuildingBlocks.Messaging.Events.ProductEvents;
using Catalog.Read.API.Repositories;
using MassTransit;

namespace Catalog.Read.API.EventHandlers;
public class ProductReviewdIntegrationEventHadnler(ICatalogRepository repository)
    : IConsumer<ProductReviewedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductReviewedIntegrationEvent> context)
    {
        var review = new ProductReview
        {
            Id = context.Message.Id,
            ProductId = context.Message.ProductId,
            Rating = context.Message.Rating,
            Comment = context.Message.Comment,
            CreatedDate = context.Message.CreatedDate,
            CustomerId = context.Message.CustomerId,
            CustomerName = context.Message.CustomerName
        };

        await repository.AddProductReviewAsync(context.Message.ProductId, review);
    }
}
