using FluentValidation;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Infrastructure.Validators;

public class IdValidator<T> : AbstractValidator<int> where T : class
{
    public IdValidator(IRepository repository) 
    {
        RuleFor<int>(id => id)
            .MustAsync(async (id, cancellationToken) =>
            {
                var entity = await repository.GetByIdAsync<T>(id, cancellationToken);

                return entity is not null;
            }).WithMessage($"{nameof(T)} with such id doesn't exist.");
    }
}