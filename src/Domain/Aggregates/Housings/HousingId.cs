using Core;

namespace Domain.Aggregates.Housings;

public class HousingId : TypedId
{
    protected HousingId() { }

    public HousingId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}