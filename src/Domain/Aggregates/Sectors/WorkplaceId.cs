using Core;

namespace Domain.Aggregates.Sectors;

public class SectorId : TypedId
{
    protected SectorId() { }

    public SectorId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}