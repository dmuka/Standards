using Core;

namespace Domain.Aggregates.Rooms;

public class RoomId(Guid value) : TypedId(value)
{
    /// <summary>
    /// Gets the room id value.
    /// </summary>
    public string Value { get; }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}