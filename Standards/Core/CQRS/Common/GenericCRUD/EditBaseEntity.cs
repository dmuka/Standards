using FluentValidation;
using MediatR;
using Standards.Core.Constants;
using Standards.Core.CQRS.Common.Attributes;
using Standards.Core.Models.Interfaces;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;
using Standards.Infrastructure.Validators;

namespace Standards.Core.CQRS.Common.GenericCRUD;

[TransactionScope]
public class EditBaseEntity
{
    public class Query<T>(T entity) : IRequest<int> where T : BaseEntity, IEntity<int>
    {
        public T Entity { get; } = entity;
    }

    public class QueryHandler<T>(
        IRepository repository, 
        ICacheService cacheService) : IRequestHandler<Query<T>, int> where T : BaseEntity, IEntity<int>, new()
    {
        public async Task<int> Handle(Query<T> request, CancellationToken cancellationToken)
        {
            var entity = new T
            {
                Name = request.Entity.Name,
                ShortName = request.Entity.ShortName,
                Comments = request.Entity.Comments
            };
            
            repository.Update(entity);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(T.GetCacheKey());

            return result;
        }
    }

    public class QueryValidator<T> : AbstractValidator<Query<T>> where T : BaseEntity, IEntity<int>
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
                        .MaximumLength(Lengths.EntityName);

                    entity.RuleFor(e => e.ShortName)
                        .NotEmpty()
                        .MaximumLength(Lengths.ShortName);
                });
        }
    }
}