using Core.Results;
using Domain.Aggregates.Grades;
using MediatR;

namespace Application.UseCases.Grades;

public class GetGradeById
{
    public class Query(GradeId gradeId) : IRequest<Result<Grade>>
    {
        public GradeId GradeId { get; set; } = gradeId;
    }
    
    public class QueryHandler(IGradeRepository repository) : IRequestHandler<Query, Result<Grade>>
    {
        public async Task<Result<Grade>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isGradeExist = await repository.ExistsAsync(query.GradeId, cancellationToken);
            
            if (!isGradeExist) return Result.Failure<Grade>(GradeErrors.NotFound(query.GradeId));
            
            var grade = await repository.GetByIdAsync(query.GradeId, cancellationToken);

            return grade;
        }
    }
}