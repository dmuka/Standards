using System.Linq.Expressions;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using FluentAssertions;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Errors;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.BaseEntities.Places;

[TestFixture]
public class GetAllTests : BaseTestFixture
{
    private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
    private const string SlidingExpirationPath = "Cache:SlidingExpiration";
        
    private IList<Place> _places;
        
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<IConfigService> _configService;
        
    private IRequestHandler<GetAllBaseEntity.Query<Place>, Result<List<Place>>> _handler;

    [SetUp]
    public void Setup()
    {
        _places = Places;

        _cancellationToken = new CancellationToken();

        _configService = new Mock<IConfigService>();
        _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
        _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetListAsync<Place>(_cancellationToken))
            .Returns(Task.FromResult(_places));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Places, It.IsAny<Expression<Func<Place, object>>[]>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(_places));

        _handler = new GetAllBaseEntity.QueryHandler<Place>(_cacheService.Object, _configService.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetAllBaseEntity.Query<Place>();

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Value.Should().BeEquivalentTo(_places);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllBaseEntity.Query<Place>();
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Value, Has.Count.EqualTo(default(int)));
    }
}