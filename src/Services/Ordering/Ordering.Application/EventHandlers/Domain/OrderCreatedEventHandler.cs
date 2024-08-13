using BuildingBlocks.Messaging.Events.OrderFullfilment;
using Mapster;
using MassTransit;
using Microsoft.FeatureManagement;

namespace Ordering.Application.EventHandlers.Domain;
public class OrderCreatedEventHandler
    (IPublishEndpoint publishEndpoint, IFeatureManager featureManager, ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    public async Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);

        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var orderCreatedIntegrationEvent = domainEvent.order.ToOrderDto();
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }

        if(await featureManager.IsEnabledAsync("UpdateInventory"))
        {
            logger.LogInformation("Update Inventory feature is enabled. Publishing event.");
            var orderCreatedIntegrationEvent = new OrderCreatedIntegrationEvent(domainEvent.order.ToOrderDto().Adapt<BuildingBlocks.Messaging.Events.OrderFullfilment.OrderDto>());
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }
    }
}
