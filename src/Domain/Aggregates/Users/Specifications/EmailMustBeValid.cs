using System.Text.RegularExpressions;
using Core;
using Core.Results;
using Domain.Aggregates.Rooms;

namespace Domain.Aggregates.Users.Specifications;

public partial class EmailMustBeValid(string email) : ISpecification
{
    public Result IsSatisfied()
    {
        const string pattern = """^(?!\.)("([^"\r\\]|\\["\r\\])*"|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$""";
    
        var regex = EmailValidateRegex();
        
        return regex.IsMatch(email) 
                ? Result.Success()
                : Result<float>.ValidationFailure(RoomErrors.WrongHeightValue);
    }

    [GeneratedRegex("""^(?!\.)("([^"\r\\]|\\["\r\\])*"|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$""", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex EmailValidateRegex();
}