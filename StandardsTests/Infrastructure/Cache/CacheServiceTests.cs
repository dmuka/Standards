using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Implementations;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Infrastructure.Cache;

[TestFixture]
public class CacheServiceTests : BaseTestFixture
{
    private const string CacheKey = "CacheKey";
    private const int ValidId = 1;
    private const int InvalidId = 10;
    
    private ICacheService _cacheService;

    private IList<Housing> _housings;
    private CancellationToken _cancellationToken;
    
    private IMemoryCache _memoryCache;
    private Mock<IRepository> _repositoryMock;
    
    [SetUp]
    public void Setup()
    {
        _cancellationToken = new CancellationToken();
        
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _memoryCache.Set(CacheKey, Housings);

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Housing>,IIncludableQueryable<Housing,object>>>(), _cancellationToken))
            .Returns(Task.FromResult(Housings));
        
        _cacheService = new CacheService(_memoryCache);
    }

    [TearDown]
    public void TearDown()
    {
        _memoryCache.Dispose();
    }

    [Test]
    public void GetOrCreateAsync_IfInputDataValid_ShouldReturnResult()
    {
        // Arrange
        // Act
        var result = _cacheService.GetOrCreateAsync(CacheKey,
            async (token) =>
            {
                var result = await _repositoryMock.Object.GetListAsync<Housing>(
                    query => query
                        .Include(h => h.Departments)
                        .Include(h => h.Rooms),
                    token);

                return result;
            }, _cancellationToken).Result;

        // Assert
        Assert.That(result, Has.Count.EqualTo(3));
    }
    
    [Test]
    public void GetOrCreateAsync_IfDataNotCached_ShouldReturnResult()
    {
        // Arrange
        const string notCached = "NotCached";
        
        // Act
        var result = _cacheService.GetOrCreateAsync(notCached,
            async (token) =>
            {
                var result = await _repositoryMock.Object.GetListAsync<Housing>(
                    query => query
                        .Include(h => h.Departments)
                        .Include(h => h.Rooms),
                    token);

                return result;
            }, _cancellationToken).Result;

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
}