using BuildingBlocks.Messaging.Events.InventoryEvents;
using Inventory.API.CQRS.Commands.DeleteProduct;

namespace Inventory.API.EventHandlers;

public class ProductDeletedEventHandler(ISender sender, ILogger<ProductDeletedEventHandler> logger)
    : IConsumer<ProductDeletedEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        await sender.Send(new DeleteProductCommand(context.Message.Id));
    }
}
