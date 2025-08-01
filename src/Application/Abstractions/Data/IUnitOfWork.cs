namespace Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task BeginTransactionAsync(CancellationToken cancellationToken);

    Task CommitAsync(CancellationToken cancellationToken);
    
    Task RollbackAsync(CancellationToken cancellationToken);
    
    Task Dispose();
}