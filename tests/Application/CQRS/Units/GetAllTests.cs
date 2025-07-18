using System.Linq.Expressions;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Units;
using Domain.Constants;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Tests.Common;
using Unit = Domain.Models.Unit;

namespace Tests.Application.CQRS.Units;

[TestFixture]
public class GetAllTests : BaseTestFixture
{
    private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
    private const string SlidingExpirationPath = "Cache:SlidingExpiration";
        
    private IList<Unit> _units;
    private List<UnitDto> _dtos;
        
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<IConfigService> _configService;
        
    private IRequestHandler<GetAll.Query, IList<UnitDto>> _handler;

    [SetUp]
    public void Setup()
    {
        _dtos = UnitDtos;
        _units = Units;

        _cancellationToken = CancellationToken.None;

        _configService = new Mock<IConfigService>();
        _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
        _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Unit>,IIncludableQueryable<Unit,object>>>(), _cancellationToken))
            .Returns(Task.FromResult(_units));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Units, It.IsAny<Expression<Func<Unit, object?>>[]>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
            .ReturnsAsync(_units);

        _handler = new GetAll.QueryHandler(_cacheService.Object, _configService.Object); 
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
        Assert.That(result, Has.Count.EqualTo(0));
    }
}