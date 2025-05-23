using FluentValidation;
using Infrastructure.Data.Repositories.Interfaces;

namespace Infrastructure.Validators;

public class IdValidator<T> : AbstractValidator<int> where T : class
{
    public IdValidator(IRepository repository) 
    {
        RuleFor(id => id)
            .MustAsync(async (id, cancellationToken) =>
            {
                var entity = await repository.GetByIdAsync<T>(id, cancellationToken);

                return entity is not null;
            }).WithMessage($"{nameof(T)} with such id doesn't exist.");
    }
}