using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Standards;
using Domain.Constants;
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
public class EditTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    
    private StandardDto _standardDto;
    private Standard? _standard;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Edit.Query, int> _handler;
    private IValidator<Edit.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _standardDto = StandardDtos[0];
        _standard = Standards[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<Standard>(IdInDb, _cancellationToken)).ReturnsAsync(_standard);
        _repositoryMock.Setup(_ => _.GetByIdAsync<Person>(IdInDb, _cancellationToken)).ReturnsAsync(Persons[0]);
        _repositoryMock.Setup(repository => repository.GetQueryable<Service>()).Returns(Services.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Characteristic>()).Returns(Characteristics.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Workplace>()).Returns(Workplaces.AsQueryable());
        _repositoryMock.Setup(_ => _.Update(_standard));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).ReturnsAsync(1);

        _cacheService = new Mock<ICacheService>();

        _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Edit.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Edit.Query(_standardDto);
        var expected = 1;

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var query = new Edit.Query(_standardDto);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Person>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.GetQueryable<Service>(), Times.Once);
        _repositoryMock.Verify(repository => repository.GetQueryable<Workplace>(), Times.Once);
        _repositoryMock.Verify(repository => repository.GetQueryable<Characteristic>(), Times.Once);
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<Standard>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Standards), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Edit.Query(_standardDto);
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
        _standardDto = null!;

        var query = new Edit.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _standardDto.Id = id;

        var query = new Edit.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _standardDto.Name = name!;

        var query = new Edit.Query(_standardDto);

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

        var query = new Edit.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _standardDto.ShortName = shortName!;

        var query = new Edit.Query(_standardDto);

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

        var query = new Edit.Query(_standardDto);

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

        var query = new Edit.Query(_standardDto);

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

        var query = new Edit.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.StandardDto.ServiceIds);
    }

    [Test]
    public void Validator_IfWorkplaceIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _standardDto.WorkplaceIds = new List<int>();

        var query = new Edit.Query(_standardDto);

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

        var query = new Edit.Query(_standardDto);

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

        var query = new Edit.Query(_standardDto);

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

        var query = new Edit.Query(_standardDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldNotHaveValidationErrorFor(_ => _.StandardDto.CalibrationInterval);
    }
}