using Core;

namespace Domain.Aggregates.Users;

public class UserId : TypedId
{
    protected UserId() { }

    public UserId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}