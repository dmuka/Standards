using Core;

namespace Domain.Aggregates.Floors;

public class FloorId : TypedId
{
    protected FloorId() { }
    
    public FloorId(Guid value) : base(value)
    {
        Value = value.ToString();
    }
    
    /// <summary>
    /// Gets the floor id value.
    /// </summary>
    public string Value { get; private set; } // Make setter private or protected
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}