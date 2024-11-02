using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.Standards;
using Standards.CQRS.Tests.Common;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.BaseEntities.Grades;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private Grade _grade;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<CreateBaseEntity.Query<Grade>, int> _handler;
    private IValidator<CreateBaseEntity.Query<Grade>> _validator;

    [SetUp]
    public void Setup()
    {
        _grade = Grades[0];

        _cancellationToken = new CancellationToken();

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.AddAsync(_grade, _cancellationToken));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new CreateBaseEntity.QueryHandler<Grade>(_repositoryMock.Object, _cacheService.Object);
        _validator = new CreateBaseEntity.QueryValidator<Grade>(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new CreateBaseEntity.Query<Grade>(_grade);
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
        var query = new CreateBaseEntity.Query<Grade>(_grade);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(default(int)));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void Validator_IfGradeIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _grade = null;

        var query = new CreateBaseEntity.Query<Grade>(_grade);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _grade.Name = name;

        var query = new CreateBaseEntity.Query<Grade>(_grade);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _grade.Name = Cases.Length201;

        var query = new CreateBaseEntity.Query<Grade>(_grade);


        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _grade.ShortName = shortName;

        var query = new CreateBaseEntity.Query<Grade>(_grade);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _grade.ShortName = Cases.Length101;

        var query = new CreateBaseEntity.Query<Grade>(_grade);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.ShortName);
    }
}