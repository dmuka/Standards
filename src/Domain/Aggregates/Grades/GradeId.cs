using Core;

namespace Domain.Aggregates.Grades;

public class GradeId : TypedId
{
    protected GradeId() { }

    public GradeId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}