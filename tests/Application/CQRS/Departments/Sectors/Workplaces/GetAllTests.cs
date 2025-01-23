using System.Linq.Expressions;
using Application.Abstractions.Cache;
using Application.Abstractions.Configuration;
using Application.CQRS.Workplaces;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using FluentAssertions;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Departments.Sectors.Workplaces;

[TestFixture]
public class GetAllTests : BaseTestFixture
{
    private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
    private const string SlidingExpirationPath = "Cache:SlidingExpiration";

    private IList<Workplace> _workplaces;
    private IList<WorkplaceDto> _dtos;
    
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<IConfigService> _configService;
    
    private IRequestHandler<GetAll.Query, IList<WorkplaceDto>> _handler;

    [SetUp]
    public void Setup()
    {
        _workplaces = Workplaces;
        _dtos = WorkplaceDtos;

        _cancellationToken = CancellationToken.None;

        _configService = new Mock<IConfigService>();
        _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
        _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Workplaces, It.IsAny<Expression<Func<Workplace, object>>[]>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
            .Returns(Task.FromResult(_workplaces));

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Workplace>,IIncludableQueryable<Workplace,object>>>(), _cancellationToken))
            .Returns(Task.FromResult(_workplaces));

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
        Assert.That(result.Count(), Is.EqualTo(0));
    }
}