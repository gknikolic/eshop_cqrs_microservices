using BuildingBlocks.DDD_Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Catalog.Write.Infrastructure.Data.Interceptors;
public class DispatchDomainEventsInterceptor(IMediator mediator)
    : SaveChangesInterceptor
{
    private List<IDomainEvent> _domainEvents;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        PrepareDomainEvents(eventData.Context).GetAwaiter().GetResult();
        var taskResult = base.SavingChanges(eventData, result);
        DispetchDomainEvents().GetAwaiter().GetResult();
        return taskResult;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await PrepareDomainEvents(eventData.Context);
        var taskResult = await base.SavingChangesAsync(eventData, result, cancellationToken);
        await DispetchDomainEvents();
        return taskResult;
    }

    public async Task PrepareDomainEvents(DbContext? context)
    {
        if (context == null) return;

        var aggregates = context.ChangeTracker
            .Entries<IAggregate>()
            .Where(a => a.Entity.DomainEvents.Any())
            .Select(a => a.Entity);

        _domainEvents = aggregates
            .SelectMany(a => a.DomainEvents)
            .ToList();

        aggregates.ToList().ForEach(a => a.ClearDomainEvents());
    }

    public async Task DispetchDomainEvents()
    {
        if (_domainEvents == null || !_domainEvents.Any()) return;

        foreach (var domainEvent in _domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}
