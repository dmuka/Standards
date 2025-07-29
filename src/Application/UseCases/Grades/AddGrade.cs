using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Grades;
using MediatR;

namespace Application.UseCases.Grades;

public class AddGrade
{
    public class Command(GradeDto2 grade) : IRequest<Result<Grade>>
    {
        public GradeDto2 GradeDto { get; set; } = grade;
    };

    public class CommandHandler(
        IGradeRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result<Grade>>
    {
        public async Task<Result<Grade>> Handle(Command command, CancellationToken cancellationToken)
        {
            var gradeCreationResult = Grade.Create(
                command.GradeDto.GradeName, 
                command.GradeDto.GradeShortName,
                command.GradeDto.Id,
                command.GradeDto.Comments);

            if (gradeCreationResult.IsFailure) return Result.Failure<Grade>(gradeCreationResult.Error);

            var grade = gradeCreationResult.Value;
            
            await repository.AddAsync(grade, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success(grade);
        }
    }
}