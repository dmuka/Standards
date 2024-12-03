using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.CQRS.Rooms;
using Domain.Constants;
using Domain.Models.DTOs;
using Domain.Models.Housings;
using FluentAssertions;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Housings.Rooms;

[TestFixture]
public class GetAllTests : BaseTestFixture
{
    private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
    private const string SlidingExpirationPath = "Cache:SlidingExpiration";

    private IList<Room> _rooms;
    private IList<RoomDto> _dtos;
    
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<IConfigService> _configService;
    
    private IRequestHandler<GetAll.Query, IEnumerable<RoomDto>> _handler;

    [SetUp]
    public void Setup()
    {
        _rooms = Rooms;
        _dtos = RoomDtos;

        _cancellationToken = new CancellationToken();

        _configService = new Mock<IConfigService>();
        _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
        _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Rooms, It.IsAny<Func<CancellationToken, Task<IList<Room>>>>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(_rooms));

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Room>,IIncludableQueryable<Room,object>>>(), _cancellationToken))
            .Returns(Task.FromResult(_rooms));

        _handler = new GetAll.QueryHandler(_repository.Object, _cacheService.Object, _configService.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetAll.Query();

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(_dtos);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnEmptyCollection()
    {
        // Arrange
        var query = new GetAll.Query();
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Count(), Is.EqualTo(default(int)));
    }
}