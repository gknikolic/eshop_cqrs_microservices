using BuildingBlocks.Messaging.Events.ProductEvents;
using Catalog.Read.API.Repositories;
using MassTransit;

namespace Catalog.Read.API.EventHandlers;
public class ProductUpdatedEventHandler(ICatalogRepository repository)
    : IConsumer<ProductUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductUpdatedIntegrationEvent> context)
    {
        var product = await repository.GetProductByIdAsync(context.Message.Id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with id {context.Message.Id} not found.");
        }

        product.Name = context.Message.Name;
        product.Description = context.Message.Description;
        product.Price = context.Message.Price;
        product.PeicesInStock = context.Message.PeicesInStock;
        product.ImageFiles = context.Message.ImagePaths;
        product.Categories = context.Message.Categories;
        product.IsActive = context.Message.IsActive;

        await repository.UpdateProductAsync(product);
    }
}
