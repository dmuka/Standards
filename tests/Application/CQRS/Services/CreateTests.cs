using Application.Abstractions.Cache;
using Application.UseCases.DTOs;
using Application.UseCases.Services;
using Domain.Models;
using Domain.Models.Services;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Services;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
    
    private Service _service;
    private ServiceDto _serviceDto;
    private ServiceType _serviceType;
    
    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheMock;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _service = Services[0];
        
        _serviceDto = ServiceDtos[0];

        _serviceType = ServiceTypes[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.AddAsync(_service, _cancellationToken));
        _repositoryMock.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));
        _repositoryMock.Setup(repository => repository.GetByIdAsync<ServiceType>(ValidId, _cancellationToken)).ReturnsAsync(_serviceType);
        _repositoryMock.Setup(repository => repository.GetQueryable<Material>()).Returns(new List<Material> { Materials[0] }.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Quantity>()).Returns(new List<Quantity> { Quantities[0] }.AsQueryable());

        _cacheMock = new Mock<ICacheService>();
        
        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheMock.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_serviceDto);
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
        var query = new Create.Query(_serviceDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfServiceDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _serviceDto = null;

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _serviceDto.Name = name;

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _serviceDto.Name = Cases.Length201;

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _serviceDto.ShortName = shortName;

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _serviceDto.ShortName = Cases.Length101;

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.ShortName);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfServiceTypeIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _serviceDto.ServiceTypeId = id;

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.ServiceTypeId);
    }

    [Test]
    public void Validator_IfMaterialIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _serviceDto.MaterialIds = new List<int>();

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.MaterialIds);
    }

    [Test]
    public void Validator_IfWMaterialIdIsNotInDb_ShouldHaveValidationError()
    {
        // Arrange
        _serviceDto.MaterialIds = new List<int> { IdNotInDb };

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.MaterialIds);
    }

    [Test]
    public void Validator_IfMaterialQuantityIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _serviceDto.MaterialsQuantityIds = new List<int>();

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.MaterialsQuantityIds);
    }

    [Test]
    public void Validator_IfMaterialQuantityIdIsNotInDb_ShouldHaveValidationError()
    {
        // Arrange
        _serviceDto.MaterialsQuantityIds = new List<int> { IdNotInDb };

        var query = new Create.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.MaterialsQuantityIds);
    }
}