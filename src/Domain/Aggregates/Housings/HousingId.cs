using Core;

namespace Domain.Aggregates.Housings;

public class HousingId : TypedId
{
    protected HousingId() { }
    
    public HousingId(Guid value) : base(value)
    {
        Value = value.ToString();
    }
    /// <summary>
    /// Gets the housing id value.
    /// </summary>
    public string Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}