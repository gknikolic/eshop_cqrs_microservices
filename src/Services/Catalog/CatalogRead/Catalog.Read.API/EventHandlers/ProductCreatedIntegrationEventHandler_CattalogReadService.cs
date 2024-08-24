using BuildingBlocks.Messaging.Events.ProductEvents;
using Catalog.Read.API.Repositories;
using MassTransit;

namespace Catalog.Read.API.EventHandlers;

public class ProductCreatedIntegrationEventHandler_CattalogReadService(ICatalogRepository repository)
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
            PeicesInStock = context.Message.Quantity,
            ImageFiles = context.Message.ImageFiles,
            Categories = context.Message.Categories,
            Color = context.Message.Color,
            Attributes = context.Message.ProductAttributes.Select(x => new ProductAttribute { Name = x.Name, Value = x.Value}).ToList()
        };

        await repository.AddProductAsync(product);
    }
}
