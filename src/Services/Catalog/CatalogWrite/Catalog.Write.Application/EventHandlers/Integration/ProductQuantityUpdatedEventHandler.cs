namespace Catalog.Write.Application.EventHandlers.Integration;
public class ProductQuantityUpdatedEventHandler : IConsumer<ProductQuantityUpdatedIntegrationEvent>
{
    public Task Consume(ConsumeContext<ProductQuantityUpdatedIntegrationEvent> context)
    {
        throw new NotImplementedException();
    }
}
