using Domain;
using FluentValidation;

namespace Application.Abstractions.Data.Validators;

public static class ValidatorExtensions
{
    public static IRuleBuilderOptions<T, TEntity> EntityMustExist<T, TEntity>(
        this IRuleBuilder<T, TEntity> ruleBuilder,
        IRepository repository)
        where TEntity : BaseEntity
    {
        return ruleBuilder.SetValidator(new TypedIdValidator<TEntity>(repository));
    }
    
    public static IRuleBuilderOptions<T, TId> EntityByIdMustExist<T, TId>(
        this IRuleBuilder<T, TId> ruleBuilder,
        IRepository repository)
        where T : BaseEntity
        where TId : struct
    {
        return ruleBuilder.SetAsyncValidator(new IdValidator<T, TId>(repository));
    }
    
    public static IRuleBuilderOptions<T, TId?> EntityByNullableIdMustExist<T, TId>(
        this IRuleBuilder<T, TId?> ruleBuilder,
        IRepository repository)
        where T : BaseEntity
        where TId : struct
    {
        return ruleBuilder.SetAsyncValidator(new NullableIdValidator<T, TId>(repository));
    }
}