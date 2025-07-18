using Domain;
using FluentValidation;
using FluentValidation.Validators;

namespace Application.Abstractions.Data.Validators;

public class NullableIdValidator<T, TId>(IRepository repository) : IAsyncPropertyValidator<T, TId?> 
    where TId : struct 
    where T : BaseEntity
{
    public async Task<bool> IsValidAsync(ValidationContext<T> context, TId? value, CancellationToken cancellation)
    {
        if (value is null) return false;

        var isExist = await repository.ExistsByIdAsync<T>(value, cancellation);

        return isExist;
    }

    public string GetDefaultMessageTemplate(string errorCode)
    {
        return "Entity with such id doesn't exist.";
    }

    public string Name { get; } = "NullableIdValidator";
}