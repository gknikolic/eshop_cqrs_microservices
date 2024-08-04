using BuildingBlocks.Messaging.Events;
using Inventory.API.CQRS.Commands.CreateProduct;
using Inventory.API.Dtos;
using Mapster;
using MassTransit;
using MediatR;

namespace Inventory.API.EventHandlers;

public class ProductCreatedEventHandler(ISender sender)
    : IConsumer<ProductCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        await sender.Send(new CreateProductCommand(context.Message.Adapt<ProductDto>()));
    }

}
