using FluentValidation;
using Standards.Core.Models.Interfaces;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Infrastructure.Validators
{
    public static class CustomValidators
    {
        public static IRuleBuilderOptions<T, int> IdValidator<T, TEntity>(
            this IRuleBuilder<T, int> ruleBuilder,
            IRepository repository)
            where TEntity : class, IEntity
        {
            return ruleBuilder.MustAsync(async (id, cancellationToken) =>
            {
                var entity = await repository.GetByIdAsync<TEntity>(id, cancellationToken);

                return entity is not null;
                }).WithMessage($"{nameof(TEntity)} with such id doesn't exist.");
        }        
        
        public static IRuleBuilderOptions<TEntity, int> IdValidator<TEntity>(
            this IRuleBuilder<TEntity, int> ruleBuilder,
            IRepository repository)
            where TEntity : class, IEntity
        {
            return ruleBuilder.MustAsync(async (id, cancellationToken) =>
            {
                var entity = await repository.GetByIdAsync<TEntity>(id, cancellationToken);

                return entity is not null;
                }).WithMessage($"{nameof(TEntity)} with such id doesn't exist.");
        }
    }
}