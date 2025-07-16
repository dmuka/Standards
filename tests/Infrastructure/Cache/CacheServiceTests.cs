using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Domain.Models.Housings;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Tests.Common;
using WebApi.Infrastructure.Services.Implementations;

namespace Tests.Infrastructure.Cache;

[TestFixture]
public class CacheServiceTests : BaseTestFixture
{
    private const string CacheKey = "CacheKey";
    private const int ValidId = 1;
    private const int InvalidId = 10;
    private readonly TimeSpan AbsoluteExpiration = new TimeSpan(0, 5, 0);
    private readonly TimeSpan SlidingExpiration = new TimeSpan(0, 2, 0);

    
    private ICacheService _cacheService;
    private IMemoryCache _memoryCache;
    
    private IList<Housing> _housings;
    
    private Mock<IRepository> _repositoryMock;
    private Mock<IQueryable<Housing>> _queryableMock;
    private CancellationToken _cancellationToken;
    
    [SetUp]
    public void Setup()
    {
        _cancellationToken = CancellationToken.None;
        
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _memoryCache.Set(CacheKey, Housings);

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.GetQueryable<Housing>()).Returns(Housings.AsQueryable);
        _repositoryMock.Setup(repository => repository.QueryableToListAsync(It.IsAny<IQueryable<Housing>>(), _cancellationToken))
             .ReturnsAsync(Housings);
        
        _queryableMock = new Mock<IQueryable<Housing>>();
        
        _cacheService = new CacheService(_memoryCache, _repositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _memoryCache.Dispose();
    }

    [Test]
    public void GetOrCreateAsync_IfInputDataValidWithExpirations_ShouldReturnResult()
    {
        // Arrange
        // Act
        var result = _cacheService.GetOrCreateAsync<Housing>(CacheKey,
            [h => h.Rooms],
            _cancellationToken,
            AbsoluteExpiration,
            SlidingExpiration).Result;

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
    }

    [Test]
    public void GetOrCreateAsync_IfInputDataValid_ShouldReturnResult()
    {
        // Arrange
        // Act
        var result = _cacheService.GetOrCreateAsync<Housing>(
            CacheKey,
            [h => h.Rooms],
            _cancellationToken).Result;

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
    }
    
    [Test]
    public void GetOrCreateAsync_IfDataNotCached_ShouldReturnResult()
    {
        // Arrange
        const string notCached = "NotCached";
        
        // Act
        var result = _cacheService.GetOrCreateAsync<Housing>(
            notCached,
            [h => h.Rooms],
            _cancellationToken).Result;

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
        Assert.That(_memoryCache.TryGetValue(notCached, out _), Is.True);
    }

    [Test]
    public void GetById_IfIdValid_ShouldReturnResult()
    {
        // Arrange
        var id = ValidId;
        
        // Act
        var result = _cacheService.GetById<Housing>(CacheKey, id);

        // Assert
        result.Should().BeEquivalentTo(Housings[0]);
    }

    [Test]
    public void GetById_IfIdInvalid_ShouldReturnNull()
    {
        // Arrange
        var id = InvalidId;
        
        // Act
        var result = _cacheService.GetById<Housing>(CacheKey, id);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Remove_IfIdValid_ShouldReturnResult()
    {
        // Arrange
        // Act
        _cacheService.Remove(CacheKey);

        // Assert
        Assert.That(_memoryCache.TryGetValue(CacheKey, out _), Is.False);
    }

    [Test]
    public void Create_IfAllDataIsValid_ShouldSetCache()
    {
        // Arrange
        _memoryCache.Remove(CacheKey);
        
        // Act
        _cacheService.Create(CacheKey, Housings);

        // Assert
        Assert.That(_memoryCache.TryGetValue(CacheKey, out _), Is.True);
    }

    [Test]
    public void Create_IfAllDataIsValidWithExpirations_ShouldSetCache()
    {
        // Arrange
        _memoryCache.Remove(CacheKey);
        
        // Act
        _cacheService.Create(CacheKey, Housings, AbsoluteExpiration, SlidingExpiration);

        // Assert
        Assert.That(_memoryCache.TryGetValue(CacheKey, out _), Is.True);
    }
    
    [Test]
    public void Create_IfCacheAlreadyExist_ShouldNotSetCache()
    {
        // Arrange
        // Act
        _cacheService.Create(CacheKey, Housings, AbsoluteExpiration, SlidingExpiration);

        // Assert
        Assert.That(_memoryCache.TryGetValue(CacheKey, out _), Is.True);
    }
}