using Core;
using Core.Results;
using Domain.Aggregates.Users.Specifications;

namespace Domain.Aggregates.Users;

public class Email : ValueObject
{
    protected Email() { }
    public string Value { get; } = null!;

    private Email(string value) => Value = value;

    public static Result<Email> Create(string email)
    {
        var emailValidationResult = new EmailMustBeValid(email).IsSatisfied();

        return emailValidationResult.IsFailure 
            ? Result.Failure<Email>(emailValidationResult.Error) 
            : Result.Success(new Email(email));
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}