using BuildingBlocks.DDD_Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;
public class DispatchDomainEventsInterceptor(IMediator mediator)
    : SaveChangesInterceptor
{
    private List<IDomainEvent> _domainEvents;

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        PrepareDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await PrepareDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
    {
        DispetchDomainEvents().GetAwaiter().GetResult();
        return base.SavedChanges(eventData, result);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispetchDomainEvents();
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    public override void SaveChangesFailed(DbContextErrorEventData eventData)
    {
        _domainEvents?.Clear(); // Clear domain events if saving changes failed
        base.SaveChangesFailed(eventData);
    }

    public override async Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        _domainEvents?.Clear(); // Clear domain events if saving changes failed
        await base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    private async Task PrepareDomainEvents(DbContext? context)
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

    private async Task DispetchDomainEvents()
    {
        if (_domainEvents == null || !_domainEvents.Any()) return;

        foreach (var domainEvent in _domainEvents)
        {
            await mediator.Publish(domainEvent);
        }
    }
}
