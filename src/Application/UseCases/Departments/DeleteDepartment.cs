using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Departments;
using MediatR;

namespace Application.UseCases.Departments;

public class DeleteDepartment
{
    public class Command(DepartmentId departmentId) : IRequest<Result>
    {
        public DepartmentId DepartmentId { get; set; } = departmentId;
    }
    
    public class CommandHandler(
        IDepartmentRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isDepartmentExist = await repository.ExistsAsync(command.DepartmentId, cancellationToken);
            
            if (!isDepartmentExist) return Result.Failure<int>(DepartmentErrors.NotFound(command.DepartmentId));
            
            var existingDepartment = await repository.GetByIdAsync(command.DepartmentId, cancellationToken: cancellationToken);
            
            repository.Remove(existingDepartment!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}