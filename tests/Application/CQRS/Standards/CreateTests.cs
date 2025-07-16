using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Standards;
using Domain.Models;
using Domain.Models.Departments;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Standards;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
    
    private Standard? _standard;
    private StandardDto _standardDto;
    
    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheMock;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _standard = Standards[0];
        
        _standardDto = StandardDtos[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.AddAsync(_standard, _cancellationToken));
        _repositoryMock.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));
        _repositoryMock.Setup(repository => repository.GetByIdAsync<Person>(ValidId, _cancellationToken)).ReturnsAsync(Persons[0]);
        _repositoryMock.Setup(repository => repository.GetQueryable<Service>()).Returns(Services.AsQueryable);
        _repositoryMock.Setup(repository => repository.GetQueryable<Characteristic>()).Returns(Characteristics.AsQueryable);
        _repositoryMock.Setup(repository => repository.GetQueryable<Workplace>()).Returns(Workplaces.AsQueryable);

        _cacheMock = new Mock<ICacheService>();
        
        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheMock.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_standardDto);
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
        var query = new Create.Query(_standardDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void StandardDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto = null;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _standardDto.Name = name;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto.Name = Cases.Length201;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _standardDto.ShortName = shortName;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto.ShortName = Cases.Length101;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.ShortName);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfResponsibleIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _standardDto.ResponsibleId = id;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.ResponsibleId);
    }

    [Test]
    public void Validator_IfServiceIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto.ServiceIds = new List<int>();

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.ServiceIds);
    }

    [Test]
    public void Validator_IfCharacteristicIdsIsNotInDb_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto.CharacteristicIds = new List<int> { IdNotInDb };

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.CharacteristicIds);
    }

    [Test]
    public void Validator_IfWorkplaceIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto.WorkplaceIds = new List<int>();

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.WorkplaceIds);
    }

    [Test]
    public void Validator_IfVerificationIntervalIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto.VerificationInterval = Cases.InvalidVerificationInterval;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.VerificationInterval);
    }

    [Test]
    public void Validator_IfCalibrationIntervalIsInvalid_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto.CalibrationInterval = Cases.InvalidCalibrationInterval;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.CalibrationInterval);
    }

    [Test]
    public void Validator_IfCalibrationIntervalIsNull_ShouldNotHaveValidationError()
    {
        // Arrange
        _standardDto.CalibrationInterval = null;

        var query = new Create.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldNotHaveValidationErrorFor(_ => _.StandardDto.CalibrationInterval);
    }
}