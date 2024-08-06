using BuildingBlocks.Messaging.Events;
using Mapster;
using MassTransit;
using Ordering.Application.CQRS.Commands.CreateProduct;

namespace Ordering.Application.EventHandlers.Integration;

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
