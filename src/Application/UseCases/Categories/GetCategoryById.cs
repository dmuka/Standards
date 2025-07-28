using Core.Results;
using Domain.Aggregates.Categories;
using MediatR;

namespace Application.UseCases.Categories;

public class GetCategoryById
{
    public class Query(CategoryId categoryId) : IRequest<Result<Category>>
    {
        public CategoryId CategoryId { get; set; } = categoryId;
    }
    
    public class QueryHandler(ICategoryRepository repository) : IRequestHandler<Query, Result<Category>>
    {
        public async Task<Result<Category>> Handle(Query query, CancellationToken cancellationToken)
        {
            var isCategoryExist = await repository.ExistsAsync(query.CategoryId, cancellationToken);
            
            if (!isCategoryExist) return Result.Failure<Category>(CategoryErrors.NotFound(query.CategoryId));
            
            var category = await repository.GetByIdAsync(query.CategoryId, cancellationToken);

            return category;
        }
    }
}