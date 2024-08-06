using BuildingBlocks.Messaging.Events;
using Inventory.API.CQRS.Commands.UpdateProductQuantity;
using MassTransit;
using MediatR;

namespace Inventory.API.EventHandlers;

public class BasketCheckoutEventHandler(ISender sender)
    : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        foreach (var item in context.Message.Products)
        {
            await sender.Send(new UpdateProductQuantityCommand(item.ProductId, -item.Quantity));
        }
    }
}
