namespace Catalog.Write.Application.EventHandlers.Integration;
public class ProductStatusUpdatedIntegrationEventHandler : IConsumer<ProductStatusUpdatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<ProductStatusUpdatedIntegrationEvent> context)
    {
        throw new NotImplementedException();
    }
}
