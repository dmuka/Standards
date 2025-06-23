using Core;

namespace Domain.Aggregates.Housings;

public class HousingId(Guid value) : TypedId(value)
{
    /// <summary>
    /// Gets the housing id value.
    /// </summary>
    public string Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}