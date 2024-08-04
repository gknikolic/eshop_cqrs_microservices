using BuildingBlocks.Messaging.Events;
using Inventory.API.CQRS.Commands.DeleteProduct;
using MassTransit;
using MediatR;

namespace Inventory.API.EventHandlers;

public class ProductDeletedEventHandler(ISender sender)
    : IConsumer<ProductDeletedEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        await sender.Send(new DeleteProductCommand(context.Message.Id));
    }
}
