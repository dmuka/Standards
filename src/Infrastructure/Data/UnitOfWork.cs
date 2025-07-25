using System.Text.Json;
using Application.Abstractions.Data;
using Core;
using Infrastructure.Data.Outbox;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data;

public class UnitOfWork(ApplicationDbContext context, ILogger<UnitOfWork> logger) : IUnitOfWork
{
    private IDbContextTransaction? _transaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken)
    {
        _transaction = await context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            var domainEvents = context.ChangeTracker
                .Entries<AggregateRoot<TypedId>>()
                .SelectMany(e => e.Entity.DomainEvents)
                .ToList();

            var outboxMessages = domainEvents
                .Select(domainEvent => new OutboxMessage
                {
                    Id = Guid.CreateVersion7(),
                    Type = domainEvent.GetType().FullName ?? domainEvent.GetType().Name,
                    Content = JsonSerializer.Serialize(domainEvent),
                    OccurredAt = domainEvent.OccuredAt
                })
                .ToList();

            await context.OutboxMessages.AddRangeAsync(outboxMessages, cancellationToken);
            
            await context.SaveChangesAsync(cancellationToken);
            if (_transaction is not null)
            {
                await _transaction.CommitAsync(cancellationToken);
                await _transaction.DisposeAsync();
            }
        }
        catch (Exception exception)
        {
            logger.LogError("Unit of work exception: {Message}", exception.InnerException);
            await RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_transaction is null) return;
        
        await _transaction.RollbackAsync(cancellationToken);
        await _transaction.DisposeAsync();
    }

    public async Task Dispose()
    {
        if (_transaction is not null) await _transaction.DisposeAsync();
        await context.DisposeAsync();
    }
}