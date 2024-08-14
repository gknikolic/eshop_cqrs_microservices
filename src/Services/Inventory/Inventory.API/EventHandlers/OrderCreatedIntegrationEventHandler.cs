using BuildingBlocks.Messaging.Events.OrderFullfilment;
using Inventory.API.CQRS.Commands.UpdateProductQuantity;
using Inventory.API.CQRS.Commands.UpdateProductQuantityBy;

namespace Inventory.API.EventHandlers;

public class OrderCreatedIntegrationEventHandler(ISender sender, ILogger<OrderCreatedIntegrationEvent> logger)
    : IConsumer<OrderCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        var pruducts = context.Message.Order.OrderItems;

        foreach (var item in pruducts)
        {
            await sender.Send(new UpdateProductQuantityByCommand(item.ProductId, item.Quantity));
        }
    }
}
