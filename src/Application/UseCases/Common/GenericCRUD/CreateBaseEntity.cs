using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.Abstractions.Data.Validators;
using Application.UseCases.Common.Attributes;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Interfaces;
using FluentValidation;
using MediatR;

namespace Application.UseCases.Common.GenericCRUD;

[TransactionScope]
public class CreateBaseEntity
{
    public class Query<T>(T entity) : IRequest<int> where T : Entity, ICacheable
    {
        public T Entity { get; } = entity;
    }

    public class QueryHandler<T>(
        IRepository repository, 
        ICacheService cacheService) : IRequestHandler<Query<T>, int> where T : Entity, ICacheable, new()
    {
        public async Task<int> Handle(Query<T> request, CancellationToken cancellationToken)
        {
            var entity = new T
            {
                Name = request.Entity.Name,
                ShortName = request.Entity.ShortName
            };
            
            if (request.Entity.Comments is not null) entity.Comments = request.Entity.Comments;
            
            await repository.AddAsync(entity, cancellationToken);
            
            var result = await repository.SaveChangesAsync(cancellationToken);
            
            cacheService.Remove(T.GetCacheKey());

            return result;
        }
    }

    public class QueryValidator<T> : AbstractValidator<Query<T>> where T : Entity, ICacheable
    {
        public QueryValidator(IRepository repository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(query => query.Entity)
                .NotEmpty()
                .ChildRules(entity =>
                {
                    entity.RuleFor(e => e.Id)
                        .GreaterThan(0)
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