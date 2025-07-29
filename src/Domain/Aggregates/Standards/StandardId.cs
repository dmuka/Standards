using Core;

namespace Domain.Aggregates.Standards;

public class StandardId : TypedId
{
    protected StandardId() { }

    public StandardId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}