namespace Domain.Aggregates.Housings
{
    public interface IHousingRepository
    {
        public Task<Housing?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        public Task<Housing[]> GetAllAsync(CancellationToken cancellationToken);

        public Task AddAsync(Housing housing, CancellationToken cancellationToken);

        public void Remove(Housing floor);

        public void Update(Housing floor);

        public Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken);
    }
}