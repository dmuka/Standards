using Core;

namespace Domain.Aggregates.Floors;

public class FloorId(Guid value) : TypedId(value)
{
    /// <summary>
    /// Gets the floor id value.
    /// </summary>
    public string Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}