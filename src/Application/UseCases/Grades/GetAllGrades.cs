using Domain.Aggregates.Grades;
using MediatR;

namespace Application.UseCases.Grades;

public class GetAllGrades
{
    public class Query : IRequest<IList<Grade>>;
    
    public class QueryHandler(IGradeRepository repository) : IRequestHandler<Query, IList<Grade>>
    {
        public async Task<IList<Grade>> Handle(Query request, CancellationToken cancellationToken)
        {
            var grades = await repository.GetAllAsync(cancellationToken);

            return grades;
        }
    }
}