using BuildingBlocks.Exceptions;
using Catalog.Write.Application.Repositories;

namespace Catalog.Write.Application.EventHandlers.Integration;
public class ProductQuantityUpdatedIntegrationEventHandler(IProductRepository productRepository)
    : IConsumer<ProductQuantityUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductQuantityUpdatedIntegrationEvent> context)
    {
        var product = await productRepository.GetAsync(context.Message.ProductId);

        product.UpdateStock(context.Message.Quantity);

        await productRepository.UpdateProductAsync(product, context.CancellationToken);
    }
}
