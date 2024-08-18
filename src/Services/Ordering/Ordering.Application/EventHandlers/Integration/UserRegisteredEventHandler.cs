using BuildingBlocks.Messaging.Events.CustomerEvents;
using Mapster;
using MassTransit;
using Ordering.Application.CQRS.Commands.CreateCustomer;

namespace Ordering.Application.EventHandlers.Integration;
public class UserRegisteredEventHandler(ISender sender)
    : IConsumer<UserRegisteredEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
    {
        var command = new CreateCustomerCommand(context.Message.Adapt<CustomerDto>());
        var result = await sender.Send(command);
    }
}
