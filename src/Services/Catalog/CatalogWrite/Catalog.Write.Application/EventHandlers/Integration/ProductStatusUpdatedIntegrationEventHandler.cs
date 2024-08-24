using BuildingBlocks.Exceptions;

namespace Catalog.Write.Application.EventHandlers.Integration;
public class ProductStatusUpdatedIntegrationEventHandler(IApplicationDbContext dbContext) 
    : IConsumer<ProductStatusUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductStatusUpdatedIntegrationEvent> context)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == context.Message.ProductId);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), context.Message.ProductId);
        }

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
