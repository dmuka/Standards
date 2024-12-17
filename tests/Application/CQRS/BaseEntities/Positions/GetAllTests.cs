using System.Linq.Expressions;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Persons;
using FluentAssertions;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Errors;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.BaseEntities.Positions;

[TestFixture]
public class GetAllTests : BaseTestFixture
{
    private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
    private const string SlidingExpirationPath = "Cache:SlidingExpiration";
        
    private IList<Position> _positions;
        
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<IConfigService> _configService;
        
    private IRequestHandler<GetAllBaseEntity.Query<Position>, Result<List<Position>>> _handler;

    [SetUp]
    public void Setup()
    {
        _positions = Positions;

        _cancellationToken = CancellationToken.None;

        _configService = new Mock<IConfigService>();
        _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
        _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetListAsync<Position>(_cancellationToken))
            .Returns(Task.FromResult(_positions));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Positions, It.IsAny<Expression<Func<Position, object>>[]>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(_positions));

        _handler = new GetAllBaseEntity.QueryHandler<Position>(_cacheService.Object, _configService.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetAllBaseEntity.Query<Position>();

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Value.Should().BeEquivalentTo(_positions);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllBaseEntity.Query<Position>();
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Value, Has.Count.EqualTo(0));
    }
}