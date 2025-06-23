using Application.Abstractions.Cache;
using Application.UseCases.DTOs;
using Application.UseCases.Services;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.Housings;
using Domain.Models.Persons;
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
public class EditTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    
    private ServiceDto _serviceDto;
    private Service _service;
    private ServiceType _serviceType;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Edit.Query, int> _handler;
    private IValidator<Edit.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _serviceDto = ServiceDtos[0];
        _service = Services[0];

        _serviceType = ServiceTypes[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<Service>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_service));
        _repositoryMock.Setup(_ => _.GetByIdAsync<ServiceType>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_serviceType));
        _repositoryMock.Setup(repository => repository.GetQueryable<Room>()).Returns(new List<Room> { Rooms[0], Rooms[1] }.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Person>()).Returns(new List<Person> { Persons[0], Persons[1], Persons[2] }.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Workplace>()).Returns(new List<Workplace> { Workplaces[4], Workplaces[5] }.AsQueryable());
        _repositoryMock.Setup(_ => _.Update(_service));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Edit.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Edit.Query(_serviceDto);
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
        var query = new Edit.Query(_serviceDto);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        _repositoryMock.Verify(repository => repository.GetByIdAsync<ServiceType>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<Service>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Services), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Edit.Query(_serviceDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void ServiceDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _serviceDto = null;

        var query = new Edit.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _serviceDto.Id = id;

        var query = new Edit.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _serviceDto.Name = name;

        var query = new Edit.Query(_serviceDto);

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

        var query = new Edit.Query(_serviceDto);

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

        var query = new Edit.Query(_serviceDto);

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

        var query = new Edit.Query(_serviceDto);

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

        var query = new Edit.Query(_serviceDto);

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

        var query = new Edit.Query(_serviceDto);

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

        var query = new Edit.Query(_serviceDto);

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

        var query = new Edit.Query(_serviceDto);

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

        var query = new Edit.Query(_serviceDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceDto.MaterialsQuantityIds);
    }
}