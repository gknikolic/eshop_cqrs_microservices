using BuildingBlocks.Messaging.Events.CustomerEvents;
using Mapster;
using MassTransit;
using Ordering.Application.CQRS.Commands.CreateCustomer;

namespace Ordering.Application.EventHandlers.Integration;
public class UserRegisteredIntegrationEventHandler_OrderingService(ISender sender)
    : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var command = new CreateCustomerCommand(context.Message.Adapt<CustomerDto>());
        var result = await sender.Send(command);
    }
}
