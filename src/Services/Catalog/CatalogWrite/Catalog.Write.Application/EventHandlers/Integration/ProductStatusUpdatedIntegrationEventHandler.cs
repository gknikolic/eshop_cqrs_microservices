using BuildingBlocks.Exceptions;
using Catalog.Write.Application.Repositories;

namespace Catalog.Write.Application.EventHandlers.Integration;
public class ProductStatusUpdatedIntegrationEventHandler(IProductRepository productRepository) 
    : IConsumer<ProductStatusUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductStatusUpdatedIntegrationEvent> context)
    {
        var product = await productRepository.GetAsync(context.Message.ProductId);

        if (context.Message.IsActive == false)
        {
            product.Deactivate();
        }
        else
        {
            product.Activate();
        }

        await productRepository.UpdateProductAsync(product, context.CancellationToken);
    }
}   
