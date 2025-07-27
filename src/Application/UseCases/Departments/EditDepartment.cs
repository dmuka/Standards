using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Departments;
using MediatR;

namespace Application.UseCases.Departments;

public class EditDepartment
{
    public class Command(DepartmentDto2 department) : IRequest<Result>
    {
        public DepartmentDto2 DepartmentDto { get; set; } = department;
    }

    public class CommandHandler(
        IDepartmentRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isDepartmentExist = await repository.ExistsAsync(command.DepartmentDto.Id, cancellationToken);
            
            if (!isDepartmentExist) return Result.Failure(DepartmentErrors.NotFound(command.DepartmentDto.Id));
            
            var existingDepartment = await repository.GetByIdAsync(command.DepartmentDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingDepartment!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}