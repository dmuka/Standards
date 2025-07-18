using System.Linq.Expressions;
using Application.Abstractions.Data;
using Application.Exceptions;
using Core;
using Domain.Services;
using Infrastructure.Exceptions.Enum;

namespace Infrastructure.Data.Repositories;

public class ChildEntityUniqueness(IRepository repository) : IChildEntityUniqueness
{
    public async Task<bool> IsUniqueAsync<TChild, TParent>(
        TypedId childEntityId, 
        TypedId parentEntityId, 
        CancellationToken cancellationToken) 
        where TChild : Entity 
        where TParent : Entity
    {
        Expression<Func<TChild, bool>> func = 
            child => ValidateChildEntity<TChild, TParent>(child, parentEntityId, childEntityId);

        return await repository.ExistsAsync(func, cancellationToken);

    }
    
    private bool ValidateChildEntity<TChild, TParent>(
        Entity child, 
        TypedId parentEntityId, 
        TypedId childEntityId)
    {
        var property = typeof(TChild).GetProperty(nameof(TParent) + "Id");
        if (property is null)
            throw new StandardsException(StatusCodeByError.InternalServerError,
                $"Wrong {nameof(TChild)} type, must have id of the parent entity.",
                "Internal server error.");
        var boxedValue = property.GetValue(child);
        if (boxedValue is null)
            throw new StandardsException(StatusCodeByError.InternalServerError,
                $"Wrong {nameof(TChild)} type, must have id of the parent entity.",
                "Internal server error.");
        var unboxedValue = (TypedId)boxedValue;
        
        return unboxedValue == parentEntityId && child.Id == childEntityId;
    }
}