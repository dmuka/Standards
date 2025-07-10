using Core;

namespace Domain.Aggregates.Persons;

public class PersonId : TypedId
{
    protected PersonId() { }

    public PersonId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}