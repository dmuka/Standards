using Core.Results;
using Domain.Aggregates.Sectors;
using MediatR;

namespace Application.UseCases.Sectors;

public class GetSectorById
{
    public class Query(SectorId sectorId) : IRequest<Result<Sector>>
    {
        public SectorId SectorId { get; set; } = sectorId;
    }
    
    public class QueryHandler(ISectorRepository repository) : IRequestHandler<Query, Result<Sector>>
    {
        public async Task<Result<Sector>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isSectorExist = await repository.ExistsAsync(query.SectorId, cancellationToken);
            
            if (!isSectorExist) return Result.Failure<Sector>(SectorErrors.NotFound(query.SectorId));
            
            var sector = await repository.GetByIdAsync(query.SectorId, cancellationToken);

            return sector;
        }
    }
}