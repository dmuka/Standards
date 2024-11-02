using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Characteristics;
using Standards.Core.Models.DTOs;
using Standards.CQRS.Tests.Common;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Characteristics;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
        
    private CharacteristicDto _characteristic;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _characteristic = CharacteristicsDtos[0];

        _cancellationToken = new CancellationToken();

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.AddAsync(_characteristic, _cancellationToken));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_characteristic);
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
        var query = new Create.Query(_characteristic);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(default(int)));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void Validator_IfCharacteristicDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _characteristic = null;

        var query = new Create.Query(_characteristic);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _characteristic.Name = name;

        var query = new Create.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;
        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _characteristic.Name = Cases.Length201;

        var query = new Create.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _characteristic.ShortName = shortName;

        var query = new Create.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _characteristic.ShortName = Cases.Length101;

        var query = new Create.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.ShortName);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfGradeIdIsInvalid_ShouldHaveValidationError(int gradeId)
    {
        // Arrange
        _characteristic.GradeId = gradeId;

        var query = new Create.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.GradeId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfStandardIdIsInvalid_ShouldHaveValidationError(int standardId)
    {
        // Arrange
        _characteristic.StandardId = standardId;

        var query = new Create.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.StandardId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfUnitdIsInvalid_ShouldHaveValidationError(int unitId)
    {
        // Arrange
        _characteristic.UnitId = unitId;

        var query = new Create.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.UnitId);
    }
}