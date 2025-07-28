using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Categories;
using MediatR;

namespace Application.UseCases.Categories;

public class AddCategory
{
    public class Command(CategoryDto2 category) : IRequest<Result<Category>>
    {
        public CategoryDto2 CategoryDto { get; set; } = category;
    };

    public class CommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result<Category>>
    {
        public async Task<Result<Category>> Handle(Command command, CancellationToken cancellationToken)
        {
            var categoryCreationResult = Category.Create(
                command.CategoryDto.CategoryName, 
                command.CategoryDto.CategoryShortName,
                command.CategoryDto.Id,
                command.CategoryDto.Comments);

            if (categoryCreationResult.IsFailure) return Result.Failure<Category>(categoryCreationResult.Error);

            var category = categoryCreationResult.Value;
            
            await repository.AddAsync(category, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
            
            return Result.Success(category);
        }
    }
}