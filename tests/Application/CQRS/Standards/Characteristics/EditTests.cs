using Application.Abstractions.Cache;
using Application.CQRS.Characteristics;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Standards;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Standards.Characteristics;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;

    private CharacteristicDto _characteristic;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Edit.Query, int> _handler;
    private IValidator<Edit.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _characteristic = CharacteristicsDtos[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<CharacteristicDto>(ValidId, _cancellationToken)).Returns(Task.FromResult(_characteristic));
        _repositoryMock.Setup(_ => _.Update(_characteristic));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();
            
        _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Edit.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Edit.Query(_characteristic);
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
        var query = new Edit.Query(_characteristic);

        // Act
        _handler.Handle(query, _cancellationToken);

        // Assert
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Standard>(ValidId, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.GetByIdAsync<global::Domain.Models.Unit>(ValidId, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Grade>(ValidId, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<Characteristic>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Characteristics), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Edit.Query(_characteristic);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfCharacteristicDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _characteristic = null;

        var query = new Edit.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _characteristic.Id = id;

        var query = new Edit.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _characteristic.Name = name;

        var query = new Edit.Query(_characteristic);

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

        var query = new Edit.Query(_characteristic);

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

        var query = new Edit.Query(_characteristic);

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

        var query = new Edit.Query(_characteristic);

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

        var query = new Edit.Query(_characteristic);

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

        var query = new Edit.Query(_characteristic);

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

        var query = new Edit.Query(_characteristic);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CharacteristicDto.UnitId);
    }
}