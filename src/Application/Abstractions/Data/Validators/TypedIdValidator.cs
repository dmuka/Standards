using Domain;
using FluentValidation;

namespace Application.Abstractions.Data.Validators;

public class TypedIdValidator<T> : AbstractValidator<T> where T : BaseEntity
{
    public TypedIdValidator(IRepository repository) 
    {
        RuleFor(entity => entity.Id)
            .MustAsync(async (id, cancellationToken) =>
            {
                var entity = await repository.GetByIdAsync<T>(id, cancellationToken);

                return entity is not null;
            }).WithMessage($"{nameof(T)} with such id doesn't exist.");
    }
}