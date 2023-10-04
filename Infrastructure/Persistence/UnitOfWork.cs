using Application.Common.Interface.Persistence;
using Domain.Common.Models;
using Infrastructure.Persistence.EFCoreRepositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly IPublisher _publisher;
    private readonly WorkWhisperDbContext _context;
    private readonly ISpaceRepository _spaceRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly IPostRepository _postRepository;
    private bool _disposed = false;

    public ISpaceRepository SpaceRepository { get {
            if (_spaceRepository is null) 
                return new SpaceRepository(_context);
            return _spaceRepository; } }
    public IMemberRepository MemberRepository { get { 
            if(_memberRepository is null)
                return new MemberRepository(_context);
            return _memberRepository; } }
    public IPostRepository PostRepository { get { 
            if(_postRepository is null)
                return new PostRepository(_context);
            return _postRepository; } }
    public UnitOfWork(WorkWhisperDbContext ctx, IPublisher publisher)
    {
        _context = ctx;
        _publisher = publisher;
    }

    private void Dispose(bool disposing)
    {
        if(!this._disposed)
        {
            if(disposing)
            {
                _context.Dispose();
            }
        }
        this._disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task SaveAsync()
    {
        await PublishDomainEvents();
       await _context.SaveChangesAsync();
    }

    private async Task PublishDomainEvents()
    {
        //Get the dbContext.
        if (_context is null) return;

        //Get entities that is to be saved.
        var entitiesWithDomainEvents = _context.ChangeTracker.Entries<IHasDomainEvents>()
            .Where(entries => entries.Entity.DomainEvents.Any())
            .Select(entry => entry.Entity).ToList();

        //Get the domain events.
        var domainEvents = entitiesWithDomainEvents.SelectMany(entity => entity.DomainEvents).ToList();

        //Clear the domain events list
        foreach (var entity in entitiesWithDomainEvents)
        {
            entity.ClearDomainEvents();
        }

        //Publish Domain events
        foreach (var domainevent in domainEvents)
        {
            await _publisher.Publish(domainevent);
        }

        return;
    }
}
