using Core;

namespace Domain.Aggregates.Workplaces;

public class WorkplaceId : TypedId
{
    protected WorkplaceId() { }

    public WorkplaceId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}