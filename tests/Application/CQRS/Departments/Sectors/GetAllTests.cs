using System.Linq.Expressions;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.CQRS.Sectors;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Persons;
using FluentAssertions;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Departments.Sectors;

[TestFixture]
public class GetAllTests : BaseTestFixture
{
    private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
    private const string SlidingExpirationPath = "Cache:SlidingExpiration";

    private IList<Sector> _sectors;
    private IList<SectorDto> _dtos;
    
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<IConfigService> _configService;
    
    private IRequestHandler<GetAll.Query, IList<SectorDto>> _handler;

    [SetUp]
    public void Setup()
    {
        _sectors = Sectors;
        _dtos = SectorDtos;

        _cancellationToken = new CancellationToken();

        _configService = new Mock<IConfigService>();
        _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
        _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Sectors, It.IsAny<Expression<Func<Sector, object>>[]>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(_sectors));

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Sector>,IIncludableQueryable<Sector,object>>>(), _cancellationToken))
            .Returns(Task.FromResult(_sectors));

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