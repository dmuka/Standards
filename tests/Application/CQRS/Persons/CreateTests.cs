using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Persons;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Persons;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
    
    private PersonDto _person;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _person = PersonDtos[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.AddAsync(_person, _cancellationToken));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_person);
        var expected = 1;

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Create.Query(_person);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfPersonDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _person = null;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfFirstNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _person.FirstName = name;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.FirstName);
    }

    [Test]
    public void Validator_IfFirstNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _person.FirstName = Cases.Length101;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.FirstName);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfMiddleNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _person.MiddleName = name;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.MiddleName);
    }

    [Test]
    public void Validator_IfMiddleNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _person.MiddleName = Cases.Length101;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.MiddleName);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfLastNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _person.LastName = name;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.LastName);
    }

    [Test]
    public void Validator_IfLastNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _person.LastName = Cases.Length101;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.LastName);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfCategoryIdIsInvalid_ShouldHaveValidationError(int categoryId)
    {
        // Arrange
        _person.CategoryId = categoryId;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.CategoryId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfPositionIdIsInvalid_ShouldHaveValidationError(int positionId)
    {
        // Arrange
        _person.PositionId = positionId;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.PositionId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfSectorIdIsInvalid_ShouldHaveValidationError(int sectorId)
    {
        // Arrange
        _person.SectorId = sectorId;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.SectorId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfUserIdIsInvalid_ShouldHaveValidationError(int userId)
    {
        // Arrange
        _person.UserId = userId;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.UserId);
    }
        
    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfRoleIsNull_ShouldHaveValidationError(string? role)
    {
        // Arrange
        _person.Role = role;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.Role);
    }
        
    [Test, TestCaseSource(nameof(NullDate))]
    public void Validator_IfBirthdayDateIsNull_ShouldHaveValidationError(DateTime birthday)
    {
        // Arrange
        _person.BirthdayDate = birthday;

        var query = new Create.Query(_person);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.PersonDto.BirthdayDate);
    }
}