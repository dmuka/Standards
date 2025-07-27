using Domain.Aggregates.Departments;
using MediatR;

namespace Application.UseCases.Departments;

public class GetAllDepartments
{
    public class Query : IRequest<IList<Department>>;
    
    public class QueryHandler(IDepartmentRepository repository) : IRequestHandler<Query, IList<Department>>
    {
        public async Task<IList<Department>> Handle(Query request, CancellationToken cancellationToken)
        {
            var departments = await repository.GetAllAsync(cancellationToken);

            return departments;
        }
    }
}