using BuildingBlocks.Messaging.Events.InventoryEvents;
using Catalog.API.CQRS.Commands.UpdateProductQuantity;
using MassTransit;

namespace Catalog.API.EventHandlers;

public class UpdateProductQuantityEventHandler(ISender sender)
    : IConsumer<ProductQuantityUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductQuantityUpdatedIntegrationEvent> context)
    {
        await sender.Send(new UpdateProductQuantityCommand(context.Message.ProductId, context.Message.Quantity));
    }
}

