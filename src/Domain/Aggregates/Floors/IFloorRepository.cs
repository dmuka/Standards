namespace Domain.Aggregates.Floors;

public interface IFloorRepository
{
    public Task<Floor?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    public Task<Floor[]> GetAllAsync(CancellationToken cancellationToken);

    public Task AddAsync(Floor game, CancellationToken cancellationToken);

    public void Remove(Floor floor);

    public void Update(Floor floor);

    public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
}