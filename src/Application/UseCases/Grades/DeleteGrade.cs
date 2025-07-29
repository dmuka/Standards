using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Grades;
using MediatR;

namespace Application.UseCases.Grades;

public class DeleteGrade
{
    public class Command(GradeId gradeId) : IRequest<Result>
    {
        public GradeId GradeId { get; set; } = gradeId;
    }
    
    public class CommandHandler(
        IGradeRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isGradeExist = await repository.ExistsAsync(command.GradeId, cancellationToken);
            
            if (!isGradeExist) return Result.Failure<int>(GradeErrors.NotFound(command.GradeId));
            
            var existingGrade = await repository.GetByIdAsync(command.GradeId, cancellationToken: cancellationToken);
            
            repository.Remove(existingGrade!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}