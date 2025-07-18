using System.Linq.Expressions;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.Abstractions.Data;
using Application.UseCases.Common.GenericCRUD;
using Core.Results;
using Domain.Constants;
using Domain.Models.Services;
using FluentAssertions;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.BaseEntities.ServiceTypes;

[TestFixture]
public class GetAllTests : BaseTestFixture
{
    private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
    private const string SlidingExpirationPath = "Cache:SlidingExpiration";
        
    private IList<ServiceType> _serviceTypes;
        
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<IConfigService> _configService;
        
    private IRequestHandler<GetAllBaseEntity.Query<ServiceType>, Result<List<ServiceType>>> _handler;

    [SetUp]
    public void Setup()
    {
        _serviceTypes = ServiceTypes;

        _cancellationToken = CancellationToken.None;

        _configService = new Mock<IConfigService>();
        _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
        _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetListAsync<ServiceType>(_cancellationToken))
            .Returns(Task.FromResult(_serviceTypes));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.ServiceTypes, It.IsAny<Expression<Func<ServiceType, object?>>[]>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(_serviceTypes));

        _handler = new GetAllBaseEntity.QueryHandler<ServiceType>(_cacheService.Object, _configService.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetAllBaseEntity.Query<ServiceType>();

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Value.Should().BeEquivalentTo(_serviceTypes);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllBaseEntity.Query<ServiceType>();
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Value, Has.Count.EqualTo(0));
    }
}