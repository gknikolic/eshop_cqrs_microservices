using BuildingBlocks.Exceptions;
using Catalog.Write.Application.Repositories;

namespace Catalog.Write.Application.EventHandlers.Integration;
public class ProductQuantityUpdatedIntegrationEventHandler(IApplicationDbContext dbContext, IProductRepository productRepository)
    : IConsumer<ProductQuantityUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductQuantityUpdatedIntegrationEvent> context)
    {
        var product = await productRepository.GetAsync(context.Message.ProductId);

        product.UpdateStock(context.Message.Quantity);

        product.AddDomainEvent(new ProductUpdatedEvent(product));

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
