using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Units;
using Domain.Models;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Units;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
    
    private UnitDto _unit;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _unit = UnitDtos[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.AddAsync(_unit, _cancellationToken));
        _repositoryMock.Setup(_ => _.GetByIdAsync<Quantity>(ValidId, _cancellationToken))
            .ReturnsAsync(Quantities[0]);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken))
            .ReturnsAsync(1);

        _cacheService = new Mock<ICacheService>();

        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_unit);
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
        var query = new Create.Query(_unit);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfUnitDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _unit = null!;

        var query = new Create.Query(_unit);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.UnitDto);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _unit.Name = name!;

        var query = new Create.Query(_unit);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.UnitDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _unit.Name = Cases.Length16;

        var query = new Create.Query(_unit);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.UnitDto.Name);
    }

    [Test]
    public void Validator_IfRuNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _unit.RuName = Cases.Length16;

        var query = new Create.Query(_unit);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.UnitDto.RuName);
    }

    [Test]
    public void Validator_IfSymbolIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _unit.Name = Cases.Length4;

        var query = new Create.Query(_unit);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.UnitDto.Symbol);
    }

    [Test]
    public void Validator_IfRuSymbolIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _unit.RuName = Cases.Length4;

        var query = new Create.Query(_unit);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.UnitDto.RuSymbol);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfQuantityIdIsInvalid_ShouldHaveValidationError(int quantityId)
    {
        // Arrange
        _unit.QuantityId = quantityId;

        var query = new Create.Query(_unit);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.UnitDto.QuantityId);
    }
}