using BuildingBlocks.Messaging.Events.InventoryEvents;
using Catalog.API.CQRS.Commands.UpdateProductQuantity;
using MassTransit;

namespace Catalog.API.EventHandlers;

public class UpdateProductQuantityEventHandler(ISender sender)
    : IConsumer<ProductQuantityUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductQuantityUpdatedEvent> context)
    {
        await sender.Send(new UpdateProductQuantityCommand(context.Message.ProductId, context.Message.Quantity));
    }
}

