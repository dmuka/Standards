using Core;
using Core.Results;
using Domain.Aggregates.Workplaces.Constants;

namespace Domain.Aggregates.Workplaces.Specifications;

public class ImagePathMustBeValid(string imagePath) : ISpecification
{
    private static readonly string[] ValidExtensions = { ".jpg", ".jpeg" };

    public Result IsSatisfied()
    {
        if (string.IsNullOrWhiteSpace(imagePath))
        {
            return Result<string>.ValidationFailure(WorkplaceErrors.EmptyImagePathValue);
        }

        if (imagePath.Length < WorkplaceConstants.ImagePathMinLength)
        {
            return Result<string>.ValidationFailure(WorkplaceErrors.ImagePathLengthTooSmall);
        }

        var extension = Path.GetExtension(imagePath);
        if (!ValidExtensions.Contains(extension))
        {
            return Result<string>.ValidationFailure(WorkplaceErrors.UnsupportedImageFile(extension));
        }
        
        if (!File.Exists(imagePath))
        {
            return Result<string>.ValidationFailure(WorkplaceErrors.SpecifiedImageFileDoesntExist(imagePath));
        }

        return Result.Success();
    }
}