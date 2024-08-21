using BuildingBlocks.Messaging.Events.OrderFullfilment;

namespace Catalog.Write.Application.EventHandlers.Integration;
public class OrderCreatedIntegrationEventHandler : IConsumer<OrderCreatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        throw new NotImplementedException();
    }
}
