using BuildingBlocks.Messaging.Events;
using Catalog.API.CQRS.Commands.UpdateProductStatus;
using MassTransit;

namespace Catalog.API.EventHandlers;

public class UpdateProductStatusEventHandler(ISender sender)
    : IConsumer<ProductStatusUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductStatusUpdatedEvent> context)
    {
        await sender.Send(new UpdateProductStatusCommand(context.Message.Id, context.Message.NewStatus));
    }
}
