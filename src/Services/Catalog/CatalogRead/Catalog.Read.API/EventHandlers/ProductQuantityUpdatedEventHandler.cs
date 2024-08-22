using BuildingBlocks.Messaging.Events.InventoryEvents;
using Catalog.Read.API.Repositories;
using MassTransit;

namespace Catalog.Read.API.EventHandlers;
public class ProductQuantityUpdatedEventHandler(ICatalogRepository repository)
    : IConsumer<ProductQuantityUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductQuantityUpdatedIntegrationEvent> context)
    {
        var product = await repository.GetProductByIdAsync(context.Message.ProductId);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with id {context.Message.ProductId} not found.");
        }

        product.PeicesInStock = context.Message.Quantity;
        await repository.UpdateProductAsync(product);
    }
}
