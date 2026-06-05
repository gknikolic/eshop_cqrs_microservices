using BuildingBlocks.Messaging.Events.ProductEvents;
using Inventory.API.CQRS.Commands.CreateProduct;
using Inventory.API.Dtos;

namespace Inventory.API.EventHandlers;

public class ProductCreatedEventIntegrationHandler_InventoryService(ISender sender, ILogger<ProductCreatedEventIntegrationHandler_InventoryService> logger)
    : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        await sender.Send(new CreateProductCommand(context.Message.Adapt<ProductDto>()));
    }

}
