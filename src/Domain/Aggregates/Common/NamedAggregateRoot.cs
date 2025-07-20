using Core;
using Core.Results;
using Domain.Aggregates.Common.ValueObjects;

namespace Domain.Aggregates.Common;

public abstract class NamedAggregateRoot<TId> : AggregateRoot<TId>
    where TId : TypedId
{
    public Name Name { get; protected set; } = null!;
    public ShortName ShortName { get; protected set; } = null!;
    public string? Comments { get; protected set; }
    
    public Result ChangeName(Name name)
    {
        Name = name;
        
        return Result.Success();
    }
    
    public Result ChangeShortName(ShortName shortName)
    {
        ShortName = shortName;
        
        return Result.Success();
    }
    
    public Result ChangeComments(string comments)
    {
        Comments = comments;
        
        return Result.Success();
    }
}