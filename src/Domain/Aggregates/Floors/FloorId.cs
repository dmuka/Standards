using Core;

namespace Domain.Aggregates.Floors;

public class FloorId : TypedId
{
    protected FloorId() { }

    public FloorId(Guid value) : base(value)
    {
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}