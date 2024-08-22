using BuildingBlocks.Messaging.Events.ProductEvents;
using Catalog.Read.API.Repositories;
using MassTransit;

namespace Catalog.Read.API.EventHandlers;
public class ProductDeletedEventHandler(ICatalogRepository repository)
    : IConsumer<ProductDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedIntegrationEvent> context)
    {
        await repository.DeleteProductAsync(context.Message.Id);
    }
}
