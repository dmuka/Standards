using Application.Abstractions.Cache;
using Application.CQRS.Common.Attributes;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Interfaces;
using FluentValidation;
using MediatR;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Errors;
using Infrastructure.Validators;

namespace Application.CQRS.Common.GenericCRUD;

[TransactionScope]
public class EditBaseEntity
{
    public class Query<T>(T entity) : IRequest<Result<int>> where T : Entity, ICacheable
    {
        public T Entity { get; } = entity;
    }

    public class QueryHandler<T>(
        IRepository repository, 
        ICacheService cacheService) : IRequestHandler<Query<T>, Result<int>> where T : Entity, ICacheable, new()
    {
        public async Task<Result<int>> Handle(Query<T> request, CancellationToken cancellationToken)
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

            return Result.Success(result);
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