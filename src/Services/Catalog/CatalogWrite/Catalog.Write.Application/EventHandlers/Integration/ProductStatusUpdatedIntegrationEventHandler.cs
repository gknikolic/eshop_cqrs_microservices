using BuildingBlocks.Exceptions;
using Catalog.Write.Application.Repositories;

namespace Catalog.Write.Application.EventHandlers.Integration;
public class ProductStatusUpdatedIntegrationEventHandler(IApplicationDbContext dbContext, IProductRepository productRepository) 
    : IConsumer<ProductStatusUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductStatusUpdatedIntegrationEvent> context)
    {
        var product = await productRepository.GetAsync(context.Message.ProductId);

        if (context.Message.Status == false)
        {
            product.Deactivate();
        }
        else
        {
            product.Activate();
        }

        product.AddDomainEvent(new ProductUpdatedEvent(product));

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}   
