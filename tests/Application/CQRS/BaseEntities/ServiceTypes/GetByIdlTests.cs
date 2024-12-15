using Application.Abstractions.Cache;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Services;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.BaseEntities.ServiceTypes;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;
        
    private IList<ServiceType> _serviceTypes;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
        
    private IRequestHandler<GetById.Query<ServiceType>, ServiceType> _handler;
    private IValidator<GetById.Query<ServiceType>> _validator;

    [SetUp]
    public void Setup()
    {
        _serviceTypes = ServiceTypes;

        _cancellationToken = new CancellationToken();

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<ServiceType>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_serviceTypes.First(_ => _.Id == IdInDb)));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetById<ServiceType>(Cache.ServiceTypes, IdInDb)).Returns(ServiceTypes[0]);

        _handler = new GetById.QueryHandler<ServiceType>(_repository.Object, _cacheService.Object);
        _validator = new GetById.QueryValidator<ServiceType>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<ServiceType>(IdInDb);
        var expected = _serviceTypes.First(_ => _.Id == IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ReturnResult(int id)
    {
        // Arrange
        var query = new GetById.Query<ServiceType>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfHousingInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<ServiceType>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(ServiceTypes[0]));
        _repository.Verify(repository => repository.GetByIdAsync<ServiceType>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById.Query<ServiceType>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
}