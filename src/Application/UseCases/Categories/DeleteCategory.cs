using Application.Abstractions.Data;
using Core.Results;
using Domain.Aggregates.Categories;
using MediatR;

namespace Application.UseCases.Categories;

public class DeleteCategory
{
    public class Command(CategoryId categoryId) : IRequest<Result>
    {
        public CategoryId CategoryId { get; set; } = categoryId;
    }
    
    public class CommandHandler(
        ICategoryRepository repository,
        IUnitOfWork unitOfWork) : IRequestHandler<Command, Result>
    {
        public async Task<Result> Handle(Command command, CancellationToken cancellationToken)
        {
            var isCategoryExist = await repository.ExistsAsync(command.CategoryId, cancellationToken);
            
            if (!isCategoryExist) return Result.Failure<int>(CategoryErrors.NotFound(command.CategoryId));
            
            var existingCategory = await repository.GetByIdAsync(command.CategoryId, cancellationToken: cancellationToken);
            
            repository.Remove(existingCategory!);
            await unitOfWork.CommitAsync(cancellationToken);

            return Result.Success();
        }
    }
}