namespace Domain.Aggregates.Workplaces.Constants;

public static class Codes
{
    public const string Unauthorized = "UserUnauthorized";
    public const string NotFound = "WorkplaceNotFound";
    public const string EmptyImagePathValue = "EmptyImagePathValue";
    public const string ImagePathLengthTooSmall = "ImagePathLengthTooSmall";
    public const string UnsupportedImageFile = "UnsupportedImageFile";
    public const string SpecifiedImageFileDoesntExist = "SpecifiedImageFileDoesntExist";
    public const string PersonAlreadyExist = "PersonAlreadyExist";
    public const string OneOfThePersonAlreadyExist = "OneOfThePersonAlreadyExist";
    public const string ThisImageAlreadySetForThisWorkplace = "ThisImageAlreadySetForThisWorkplace";
    public const string ThisPersonAlreadySetAsResponsibleForThisWorkplace = "ThisPersonAlreadySetAsResponsibleForThisWorkplace";
}