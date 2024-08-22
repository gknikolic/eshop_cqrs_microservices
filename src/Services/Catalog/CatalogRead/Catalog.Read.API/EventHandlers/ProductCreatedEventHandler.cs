using BuildingBlocks.Messaging.Events.ProductEvents;
using Catalog.Read.API.Repositories;
using MassTransit;

namespace Catalog.Read.API.EventHandlers;

public class ProductCreatedEventHandler(ICatalogRepository repository)
    : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        var product = new Product
        {
            Id = context.Message.Id,
            Name = context.Message.Name,
            Description = context.Message.Description,
            Price = context.Message.Price,
            PeicesInStock = context.Message.PeicesInStock,
            ImageFiles = context.Message.ImagePaths,
            Categories = context.Message.Categories,
            IsActive = context.Message.IsActive,
        };

        await repository.AddProductAsync(product);
    }
}
