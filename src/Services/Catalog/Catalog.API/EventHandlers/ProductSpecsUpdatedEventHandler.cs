using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Catalog.API.EventHandlers;

public class ProductSpecsUpdatedEventHandler(ISender sender)
    : IConsumer<ProductSpecsUpdatedEvent>
{
    public async Task Consume(ConsumeContext<ProductSpecsUpdatedEvent> context)
    {
        throw new NotImplementedException();
    }
}
