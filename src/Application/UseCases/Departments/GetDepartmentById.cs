using Core.Results;
using Domain.Aggregates.Departments;
using MediatR;

namespace Application.UseCases.Departments;

public class GetDepartmentById
{
    public class Query(DepartmentId departmentId) : IRequest<Result<Department>>
    {
        public DepartmentId DepartmentId { get; set; } = departmentId;
    }
    
    public class QueryHandler(IDepartmentRepository repository) : IRequestHandler<Query, Result<Department>>
    {
        public async Task<Result<Department>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isDepartmentExist = await repository.ExistsAsync(query.DepartmentId, cancellationToken);
            
            if (!isDepartmentExist) return Result.Failure<Department>(DepartmentErrors.NotFound(query.DepartmentId));
            
            var department = await repository.GetByIdAsync(query.DepartmentId, cancellationToken);

            return department;
        }
    }
}