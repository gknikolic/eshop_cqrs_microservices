using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Catalog.API.EventHandlers;

public class UpdateProductQuantityEventHandler(ISender sender)
    : IConsumer<ProductQuantityUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductQuantityUpdatedEvent> context)
    {
        throw new System.NotImplementedException();
    }
}

