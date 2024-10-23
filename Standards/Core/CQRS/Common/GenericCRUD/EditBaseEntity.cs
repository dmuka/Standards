using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Common.GenericCRUD;

[TransactionScope]
public class EditBaseEntity<T> where T : BaseEntity, new()
{
    public class Query(T entity) : IRequest<int>
    {
        public T Entity { get; } = entity;
    }

    public class QueryHandler(
        IRepository repository, 
        ICacheService cacheService, 
        string cacheKey) : IRequestHandler<Query, int>
    {
        public async Task<int> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = new T
            {
                Name = request.Entity.Name,
                ShortName = request.Entity.ShortName,
                Comments = request.Entity.Comments
            };
            
            repository.Update(entity);
            
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

            RuleFor(query => query.Entity)
                .NotEmpty()
                .ChildRules(entity =>
                {
                    entity.RuleFor(e => e.Id)
                        .GreaterThan(default(int))
                        .SetValidator(new IdValidator<T>(repository));

                    entity.RuleFor(e => e.Name)
                        .NotEmpty()
                        .Length(Lengths.EntityName);

                    entity.RuleFor(e => e.ShortName)
                        .NotEmpty()
                        .Length(Lengths.ShortName);
                });
        }
    }
}