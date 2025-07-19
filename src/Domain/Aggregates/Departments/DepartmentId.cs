using Core;

namespace Domain.Aggregates.Departments;

public class DepartmentId : TypedId
{
    protected DepartmentId() { }

    public DepartmentId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}