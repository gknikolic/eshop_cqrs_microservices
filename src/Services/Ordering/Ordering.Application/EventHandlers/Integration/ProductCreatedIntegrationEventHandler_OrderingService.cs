using BuildingBlocks.Messaging.Events.ProductEvents;
using Mapster;
using MassTransit;
using Microsoft.Extensions.Logging;
using Ordering.Application.CQRS.Commands.CreateProduct;

namespace Ordering.Application.EventHandlers.Integration;

public class ProductCreatedIntegrationEventHandler_OrderingService(ISender sender, ILogger<ProductCreatedIntegrationEventHandler_OrderingService> logger)
    : IConsumer<ProductCreatedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedIntegrationEvent> context)
    {
        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

        var message = context.Message;

        var command = message.Adapt<CreateProductCommand>();

        var result = await sender.Send(command);
    }
}
