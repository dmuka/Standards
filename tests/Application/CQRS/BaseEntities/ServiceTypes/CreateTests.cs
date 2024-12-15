using Application.Abstractions.Cache;
using Application.CQRS.Common.GenericCRUD;
using Domain.Models.Services;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.BaseEntities.ServiceTypes;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private ServiceType _serviceType;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<CreateBaseEntity.Query<ServiceType>, int> _handler;
    private IValidator<CreateBaseEntity.Query<ServiceType>> _validator;

    [SetUp]
    public void Setup()
    {
        _serviceType = ServiceTypes[0];

        _cancellationToken = new CancellationToken();

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.AddAsync(_serviceType, _cancellationToken));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new CreateBaseEntity.QueryHandler<ServiceType>(_repositoryMock.Object, _cacheService.Object);
        _validator = new CreateBaseEntity.QueryValidator<ServiceType>(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new CreateBaseEntity.Query<ServiceType>(_serviceType);
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
        var query = new CreateBaseEntity.Query<ServiceType>(_serviceType);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(default(int)));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void Validator_IfPositionIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _serviceType = null;

        var query = new CreateBaseEntity.Query<ServiceType>(_serviceType);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _serviceType.Name = name;
        
        var query = new CreateBaseEntity.Query<ServiceType>(_serviceType);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _serviceType.Name = Cases.Length201;
        
        var query = new CreateBaseEntity.Query<ServiceType>(_serviceType);


        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _serviceType.ShortName = shortName;

        var query = new CreateBaseEntity.Query<ServiceType>(_serviceType);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _serviceType.ShortName = Cases.Length101;

        var query = new CreateBaseEntity.Query<ServiceType>(_serviceType);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.ShortName);
    }
}