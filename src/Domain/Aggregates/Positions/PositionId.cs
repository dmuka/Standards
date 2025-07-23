using Core;

namespace Domain.Aggregates.Positions;

public class PositionId : TypedId
{
    protected PositionId() { }

    public PositionId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}