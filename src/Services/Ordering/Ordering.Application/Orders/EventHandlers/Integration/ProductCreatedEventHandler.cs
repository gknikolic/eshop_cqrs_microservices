using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;
using Ordering.Application.Products.Commands;

namespace Ordering.Application.Orders.EventHandlers.Integration;

public class ProductCreatedEventHandler(ISender sender) 
    : IConsumer<ProductCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var message = context.Message;

        var command = message.Adapt<CreateProductCommand>();

        var result = await sender.Send(command);
    }
}
