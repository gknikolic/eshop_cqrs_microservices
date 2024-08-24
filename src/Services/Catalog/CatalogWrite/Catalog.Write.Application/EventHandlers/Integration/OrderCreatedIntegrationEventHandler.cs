using BuildingBlocks.Exceptions;
using BuildingBlocks.Messaging.Events.OrderFullfilment;
using System.Linq;

namespace Catalog.Write.Application.EventHandlers.Integration;
public class OrderCreatedIntegrationEventHandler(IApplicationDbContext dbContext)
    : IConsumer<OrderCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<OrderCreatedIntegrationEvent> context)
    {
        var products = await dbContext.Products.Where(x => context.Message.Order.OrderItems.Select(x => x.ProductId).Contains(x.Id)).ToListAsync();

        if (products != null && products.Any()) 
        {
            if (products.Count != context.Message.Order.OrderItems.Count)
            {
                throw new NotFoundException(@$"Not all product found from order: orderItems={context.Message.Order.OrderItems.Count} found={products.Count}.
                Missing products with ids: {string.Join(", ", context.Message.Order.OrderItems.Select(x => x.ProductId).Except(products.Select(x => x.Id.Value)))}");
            }
        }

        foreach(var orderLine in context.Message.Order.OrderItems)
        {
            var product = products.FirstOrDefault(x => x.Id == orderLine.ProductId);
            product.UpdateStock(product.Stock.Quantity - orderLine.Quantity);
            product.AddDomainEvent(new ProductUpdatedEvent(product));
        }

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
