using Core.Results;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Housings;
using Domain.Aggregates.Sectors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UseCases.Housings;

public class GetDepartmentsByHousingId
{
    public class Query(HousingId housingId) : IRequest<Result<List<Department>>>
    {
        public HousingId HousingId { get; set; } = housingId;
    }
    
    public class QueryHandler(
        IHousingRepository housingRepository, 
        IDepartmentRepository departmentRepository,
        ISectorRepository sectorRepository) : IRequestHandler<Query, Result<List<Department>>>
    {
        public async Task<Result<List<Department>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isHousingExist = await housingRepository.ExistsAsync(query.HousingId, cancellationToken);
            if (!isHousingExist) return Result.Failure<List<Department>>(HousingErrors.NotFound(query.HousingId));

            var departmentIds = (await sectorRepository.GetAllAsync(cancellationToken))
                .Select(sector => sector.DepartmentId)
                .Distinct();

            var enumerable = departmentIds as DepartmentId[] ?? departmentIds.ToArray();
            if (enumerable.Length == 0) return Result.Success<List<Department>>([]);

            var departments = await departmentRepository
                .Where(department => enumerable.Contains(department.Id))
                .ToListAsync(cancellationToken);

            return departments;
        }
    }
}