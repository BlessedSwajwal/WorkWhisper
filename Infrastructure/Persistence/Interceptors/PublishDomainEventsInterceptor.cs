using Domain.Common.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Interceptors;

public class PublishDomainEventsInterceptor : SaveChangesInterceptor
{
    private readonly IPublisher _publisher;

    public PublishDomainEventsInterceptor(IPublisher publisher)
    {
        _publisher = publisher;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        PublishDomainEvents(eventData.Context).GetAwaiter().GetResult();
        return base.SavingChanges(eventData, result);
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        await PublishDomainEvents(eventData.Context);
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private async Task PublishDomainEvents(DbContext? dbContext)
    {
        //Get the dbContext.
        if (dbContext is null) return;

        //Get entities that is to be saved.
        var entitiesWithDomainEvents = dbContext.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entries => entries.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity).ToList();

        //Get the domain events.
        var domainEvents = entitiesWithDomainEvents.Select(entity => entity.DomainEvents).ToList();

        //Clear the domain events list
        foreach(var entity in entitiesWithDomainEvents)
        {
            entity.ClearDomainEvents();
        }

        //Publish Domain events
        foreach(var domainevent in domainEvents)
        {
            await _publisher.Publish(domainevent);
        }

        return;
    }
}
