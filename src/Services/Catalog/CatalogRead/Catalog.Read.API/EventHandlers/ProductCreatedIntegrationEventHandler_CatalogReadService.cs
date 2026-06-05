using BuildingBlocks.Messaging.Events.ProductEvents;
using Catalog.Read.API.Repositories;
using MassTransit;

namespace Catalog.Read.API.EventHandlers;

public class ProductCreatedIntegrationEventHandler_CatalogReadService(ICatalogRepository repository)
    : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var product = new Product
        {
            Id = context.Message.Id,
            Name = context.Message.Name,
            Sku = context.Message.Sku,
            Description = context.Message.Description,
            Price = context.Message.Price,
            PeicesInStock = context.Message.Quantity,
            ImageFiles = context.Message.ImageFiles,
            Categories = context.Message.Categories,
            Color = context.Message.Color,
            ProductAttributes = context.Message.ProductAttributes.Select(x => new ProductAttribute { Name = x.Name, Value = x.Value }).ToList(),
            IsActive = context.Message.IsActive,
            ProductReviews = context.Message.ProductReviews.Select(x => new ProductReview
            {
                Comment = x.Comment,
                CustomerId = x.CustomerId,
                CustomerName = x.CustomerName,
                Rating = x.Rating,
                CreatedDate = x.CreatedDate,
                Id = x.Id,
                ProductId = x.ProductId
            }).ToList()
        };

        await repository.AddProductAsync(product);
    }
}
