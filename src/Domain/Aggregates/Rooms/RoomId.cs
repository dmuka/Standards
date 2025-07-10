using Core;

namespace Domain.Aggregates.Rooms;

public class RoomId : TypedId
{
    protected RoomId() { }

    public RoomId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}