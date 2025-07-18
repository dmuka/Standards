using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Materials;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Services.Materials;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
    
    private MaterialDto _material;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _material = MaterialDtos[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.AddAsync(_material, _cancellationToken));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_material);
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
        var query = new Create.Query(_material);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfMaterialDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _material = null!;

        var query = new Create.Query(_material);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.MaterialDto);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _material.Name = name!;

        var query = new Create.Query(_material);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.MaterialDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _material.Name = Cases.Length201;

        var query = new Create.Query(_material);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.MaterialDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _material.ShortName = shortName!;

        var query = new Create.Query(_material);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.MaterialDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _material.ShortName = Cases.Length101;

        var query = new Create.Query(_material);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.MaterialDto.ShortName);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfUnitIsInvalid_ShouldHaveValidationError(int unitId)
    {
        // Arrange
        _material.UnitId = unitId;

        var query = new Create.Query(_material);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.MaterialDto.UnitId);
    }
}