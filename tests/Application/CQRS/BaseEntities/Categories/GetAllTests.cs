using System.Linq.Expressions;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Persons;
using FluentAssertions;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.BaseEntities.Categories;

[TestFixture]
public class GetAllTests : BaseTestFixture
{
    private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
    private const string SlidingExpirationPath = "Cache:SlidingExpiration";
        
    private IList<Category> _categories;
        
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<IConfigService> _configService;
        
    private IRequestHandler<GetAllBaseEntity.Query<Category>, IList<Category>> _handler;

    [SetUp]
    public void Setup()
    {
        _categories = Categories;

        _cancellationToken = new CancellationToken();

        _configService = new Mock<IConfigService>();
        _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
        _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetListAsync<Category>(_cancellationToken))
            .Returns(Task.FromResult(_categories));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Categories, It.IsAny<Expression<Func<Category, object>>[]>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(_categories));

        _handler = new GetAllBaseEntity.QueryHandler<Category>(_repository.Object, _cacheService.Object, _configService.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetAllBaseEntity.Query<Category>();

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(_categories);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAllBaseEntity.Query<Category>();
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Has.Count.EqualTo(default(int)));
    }
}