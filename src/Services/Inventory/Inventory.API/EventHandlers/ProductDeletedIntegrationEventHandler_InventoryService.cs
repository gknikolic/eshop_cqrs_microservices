using BuildingBlocks.Messaging.Events.ProductEvents;
using Inventory.API.CQRS.Commands.DeleteProduct;

namespace Inventory.API.EventHandlers;

public class ProductDeletedIntegrationEventHandler_InventoryService(ISender sender, ILogger<ProductDeletedIntegrationEventHandler_InventoryService> logger)
    : IConsumer<ProductDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        await sender.Send(new DeleteProductCommand(context.Message.Id));
    }
}
