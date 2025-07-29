using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Grades;
using MediatR;

namespace Application.UseCases.Grades;

public class EditGrade
{
    public class Command(GradeDto2 grade) : IRequest<Result>
    {
        public GradeDto2 GradeDto { get; set; } = grade;
    }

    public class CommandHandler(
        IGradeRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isGradeExist = await repository.ExistsAsync(command.GradeDto.Id, cancellationToken);
            
            if (!isGradeExist) return Result.Failure(GradeErrors.NotFound(command.GradeDto.Id));
            
            var existingGrade = await repository.GetByIdAsync(command.GradeDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingGrade!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}