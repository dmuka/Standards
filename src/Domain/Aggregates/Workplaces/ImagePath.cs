using Core;
using Core.Results;
using Domain.Aggregates.Workplaces.Specifications;

namespace Domain.Aggregates.Workplaces;

public class ImagePath : ValueObject
{
    protected ImagePath() { }
    public string Value { get; }

    private ImagePath(string value) => Value = value;

    public static Result<ImagePath> Create(string imagePath)
    {
        var imagePathValidationResult = new ImagePathMustBeValid(imagePath).IsSatisfied();

        return imagePathValidationResult.IsFailure 
            ? Result<ImagePath>.ValidationFailure(imagePathValidationResult.Error) 
            : Result.Success(new ImagePath(imagePath));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}