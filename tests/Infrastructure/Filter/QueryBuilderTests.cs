using Domain.Models.Housings;
using FluentAssertions;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Filter.Implementations;
using Infrastructure.Filter.Interfaces;
using Moq;
using Tests.Common;

namespace Tests.Infrastructure.Filter;

[TestFixture]
public class QueryBuilderTests : BaseTestFixture
{
    private IQueryBuilder<Housing> _queryBuilder;
    
    private QueryParameters _parameters;
    private IList<Housing> _housings;
    private const string SearchString = "Housing1";

    private Mock<IRepository> _repositoryMock;
    
    [SetUp]
    public void Setup()
    {
        _parameters = new QueryParameters();

        var departments = Departments;

        var rooms = Rooms;

        _housings = Housings;
        
        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.GetQueryable<Housing>()).Returns(_housings.AsQueryable);

        _queryBuilder = new QueryBuilder<Housing>(_repositoryMock.Object);
    }

    [Test]
    public void Execute_IfAllDataValid_ShouldReturnValidResult()
    {
        // Arrange
        var query = _queryBuilder.Execute(_parameters);
        
        // Act
        var result = query.AsEnumerable().ToList();
        
        // Assert
        Assert.That(result, Is.EquivalentTo(_housings));
    }

    [Test]
    public void Execute_IfSortDescendingIsTrue_ShouldReturnCollectionInReverseOrder()
    {
        // Arrange
        _parameters.SortDescending = true;
        var query = _queryBuilder.Execute(_parameters);
        var expected = _housings.OrderByDescending<Housing, string>(housing => housing.Name);
        
        // Act
        var result = query.AsEnumerable().ToList();
        
        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Execute_IfSearchStringIsNotEmpty_ShouldReturnValidResult()
    {
        // Arrange
        _parameters.SearchString = SearchString;
        var query = _queryBuilder.Execute(_parameters);
        var expected = _housings.Where(housing => housing.Name == SearchString);
        
        // Act
        var result = query.AsEnumerable().ToList();
        
        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Execute_IfPaginationValuesAreSet_ShouldReturnValidResult()
    {
        // Arrange
        _parameters.PageNumber = 3;
        _parameters.ItemsOnPage = 1;
        var query = _queryBuilder.Execute(_parameters);
        var expected = _housings.Where(housing => housing.Id == 3);
        
        // Act
        var result = query.AsEnumerable().ToList();
        
        // Assert
        result.Should().BeEquivalentTo(expected);
    }
}