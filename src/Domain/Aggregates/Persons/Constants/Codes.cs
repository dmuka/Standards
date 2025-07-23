namespace Domain.Aggregates.Persons.Constants;

public static class Codes
{
    public const string Unauthorized = "UserUnauthorized";
    public const string NotFound = "PersonNotFound";
    public const string WrongFirstNameValue = "WrongFirstNameValue";
    public const string WrongMiddleNameValue = "WrongMiddleNameValue";
    public const string WrongLastNameValue = "WrongFirstNameValue";
    public const string WrongAgeValue = "WrongAgeValue";
    public const string PersonAlreadyExist = "PersonAlreadyExist";
    public const string OneOfThePersonAlreadyExist = "OneOfThePersonAlreadyExist";
    public const string WorkplaceAlreadyExist = "WorkplaceAlreadyExist";
    public const string OneOfTheWorkplaceAlreadyExist = "OneOfTheWorkplaceAlreadyExist";
    public const string ThisPositionAlreadySetForThisPerson = "ThisPositionAlreadySetForThisPerson";
    public const string ThisSectorAlreadySetForThisPerson = "ThisSectorAlreadySetForThisPerson";
}