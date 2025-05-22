using Application.Abstractions.Cache;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Services;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Errors;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.BaseEntities.ServiceTypes;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;

    private ServiceType _serviceType;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<EditBaseEntity.Query<ServiceType>, Result<int>> _handler;
    private IValidator<EditBaseEntity.Query<ServiceType>> _validator;

    [SetUp]
    public void Setup()
    {
        _serviceType = ServiceTypes[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<ServiceType>(ValidId, _cancellationToken)).Returns(Task.FromResult(_serviceType));
        _repositoryMock.Setup(_ => _.Update(_serviceType));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();
            
        _handler = new EditBaseEntity.QueryHandler<ServiceType>(_repositoryMock.Object, _cacheService.Object);
        _validator = new EditBaseEntity.QueryValidator<ServiceType>(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);
        var expected = 1;

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Value, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);

        // Act
        _handler.Handle(query, _cancellationToken);

        // Assert
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<ServiceType>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.ServiceTypes), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnZero()
    {
        // Arrange
        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Value, Is.EqualTo(0));
    }

    [Test]
    public void ServiceTypeIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _serviceType = null;

        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _serviceType.Id = default;

        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _serviceType.Name = name;

        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);

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

        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);

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

        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);

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

        var query = new EditBaseEntity.Query<ServiceType>(_serviceType);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.ShortName);
    }
}