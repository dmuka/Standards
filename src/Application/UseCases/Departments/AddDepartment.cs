using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Departments;
using MediatR;

namespace Application.UseCases.Departments;

public class AddDepartment
{
    public class Command(DepartmentDto2 department) : IRequest<Result<Department>>
    {
        public DepartmentDto2 DepartmentDto { get; set; } = department;
    };

    public class CommandHandler(
        IDepartmentRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result<Department>>
    {
        public async Task<Result<Department>> Handle(Command command, CancellationToken cancellationToken)
        {
            var departmentCreationResult = Department.Create(
                command.DepartmentDto.DepartmentName, 
                command.DepartmentDto.DepartmentShortName,
                command.DepartmentDto.Id,
                command.DepartmentDto.Comments);

            if (departmentCreationResult.IsFailure) return Result.Failure<Department>(departmentCreationResult.Error);

            var department = departmentCreationResult.Value;
            
            await repository.AddAsync(department, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success(department);
        }
    }
}