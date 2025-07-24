using Core;
using Core.Results;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Persons.Specifications;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Sectors.Events.Domain;
using Domain.Aggregates.Users.Specifications;
using Domain.Aggregates.Workplaces;
using Domain.Constants;
using Domain.Models.Interfaces;

namespace Domain.Aggregates.Users;

public class User : AggregateRoot<UserId>, ICacheable
{
    protected User() { }

    public FirstName FirstName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public PersonId PersonId { get; private set; } = null!;

    private User(
        UserId userId,
        FirstName firstName,
        LastName lastName,
        Email email,
        PersonId personId,
        string? comments)
    {
        Id = userId;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PersonId = personId;
        Comments = comments;
    }

    public static Result<User> Create(
        Guid userId,
        string firstName,
        string lastName,
        string email,
        Guid? personId = null,
        string? comments = null)
    {
        var validationResults = ValidateUserDetails(firstName, lastName, email);
        if (validationResults.Length != 0)
            return Result<User>.ValidationFailure(ValidationError.FromResults(validationResults));
        
        var user = new User( 
            new UserId(userId),
            FirstName.Create(firstName).Value,
            LastName.Create(lastName).Value,
            Email.Create(email).Value,
            personId is null ? new PersonId(Guid.CreateVersion7()) : new PersonId(personId.Value),
            comments);
            
        return Result.Success(user);
    } 
    
    public static string GetCacheKey()
    {
        return Cache.Sectors;
    }

    /// <summary>
    /// Validates user details.
    /// </summary>
    private static Result[] ValidateUserDetails(
        string firstName, 
        string lastName,
        string email)
    {
        var validationResults = new []
        {
            new FirstNameMustBeValid(firstName).IsSatisfied(),
            new LastNameMustBeValid(lastName).IsSatisfied(),
            new EmailMustBeValid(email).IsSatisfied()
        };
            
        var results = validationResults.Where(result => result.IsFailure);

        return results.ToArray();
    }
}