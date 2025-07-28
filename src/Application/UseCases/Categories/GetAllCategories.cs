using Domain.Aggregates.Categories;
using MediatR;

namespace Application.UseCases.Categories;

public class GetAllCategories
{
    public class Query : IRequest<IList<Category>>;
    
    public class QueryHandler(ICategoryRepository repository) : IRequestHandler<Query, IList<Category>>
    {
        public async Task<IList<Category>> Handle(Query request, CancellationToken cancellationToken)
        {
            var categories = await repository.GetAllAsync(cancellationToken);

            return categories;
        }
    }
}