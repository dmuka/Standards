using Core;

namespace Domain.Aggregates.Categories;

public class CategoryId : TypedId
{
    protected CategoryId() { }

    public CategoryId(Guid value) : base(value)
    {
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}