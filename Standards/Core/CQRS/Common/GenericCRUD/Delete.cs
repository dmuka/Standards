using FluentValidation;
using MediatR;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Common.GenericCRUD;

[TransactionScope]
public class Delete<T> where T : BaseEntity
{
    public class Query(int id) : IRequest<int>
    {
        public int Id { get; } = id;
    }

    public class QueryHandler(
        IRepository repository, 
        ICacheService cacheService, 
        string cacheKey) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await repository.GetByIdAsync<T>(request.Id, cancellationToken);

            await repository.DeleteAsync(entity, cancellationToken);

            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(cacheKey);

            return result;
        }
    }

    public class QueryValidator : AbstractValidator<Query>
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.Id)
                .GreaterThan(default(int))
                .SetValidator(new IdValidator<T>(repository));
        }
    }
}