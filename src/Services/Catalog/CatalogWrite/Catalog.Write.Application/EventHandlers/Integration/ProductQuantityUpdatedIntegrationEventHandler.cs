using BuildingBlocks.Exceptions;

namespace Catalog.Write.Application.EventHandlers.Integration;
public class ProductQuantityUpdatedIntegrationEventHandler(IApplicationDbContext dbContext)
    : IConsumer<ProductQuantityUpdatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductQuantityUpdatedIntegrationEvent> context)
    {
        var product = await dbContext.Products.FirstOrDefaultAsync(x => x.Id == context.Message.ProductId);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), context.Message.ProductId);
        }

        product.UpdateStock(context.Message.Quantity);

        product.AddDomainEvent(new ProductUpdatedEvent(product));

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
