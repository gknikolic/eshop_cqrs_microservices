using BuildingBlocks.Messaging.Events.CustomerEvents;

public class UserRegisteredIntegrationEventHandler_CatalogWriteService(IApplicationDbContext dbContext)
    : IConsumer<UserRegisteredIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var user = new Customer(context.Message.Id, context.Message.Name, context.Message.Email);

        await dbContext.Customers.AddAsync(user);
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
