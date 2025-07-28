using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Categories;
using MediatR;

namespace Application.UseCases.Categories;

public class EditCategory
{
    public class Command(CategoryDto2 category) : IRequest<Result>
    {
        public CategoryDto2 CategoryDto { get; set; } = category;
    }

    public class CommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isCategoryExist = await repository.ExistsAsync(command.CategoryDto.Id, cancellationToken);
            
            if (!isCategoryExist) return Result.Failure(CategoryErrors.NotFound(command.CategoryDto.Id));
            
            var existingCategory = await repository.GetByIdAsync(command.CategoryDto.Id, cancellationToken: cancellationToken);
            
            repository.Update(existingCategory!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}