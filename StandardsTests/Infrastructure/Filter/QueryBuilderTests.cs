using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;

namespace Standards.CQRS.Tests.Infrastructure.Filter;

[TestFixture]
public class QueryBuilderTests
{
    private QueryParameters _parameters;
    private IList<Housing> _housings;
    private string searchString = "Housing1";

    private IQueryBuilder<Housing> _queryBuilder;
    
    private Mock<IRepository> _repositoryMock;
    
    [SetUp]
    public void Setup()
    {
        _parameters = new QueryParameters();

        var departments = new List<Department>
        {
            new()
            {
                Id = 1,
                Name = "Name1",
                ShortName = "ShortName1",
                Housings = _housings,
                Comments = "Comments1",
                Sectors = new List<Sector>()
            }
        };

        var rooms = new List<Room>
        {
            new()
            {
                Id = 1,
                Name = "Name1",
                ShortName = "ShortName1",
                Comments = "Comments1",
                Floor = 1,
                Height = 2d,
                Length = 5d,
                Width = 4d,
                Persons = new List<Person>(),
                Sector = new Sector(),
                WorkPlaces = new List<WorkPlace>()
            }
        };

        _housings = new List<Housing>
        {
            new()
            {
                Id = 1,
                Name = "Name1",
                ShortName = "ShortName1",
                Address = "Address1",
                Comments = "Comments1",
                Departments = departments,
                FloorsCount = 1,
                Rooms = rooms
            },
            new()
            {
                Id = 2,
                Name = "Name2",
                ShortName = "ShortName1",
                Address = "Address1",
                Comments = "Comments1",
                Departments = departments,
                FloorsCount = 1,
                Rooms = rooms
            },
            new()
            {
                Id = 3,
                Name = "Name3",
                ShortName = "ShortName1",
                Address = "Address1",
                Comments = "Comments1",
                Departments = departments,
                FloorsCount = 1,
                Rooms = rooms
            }
        };

        rooms[0].Housing = _housings[0];
        
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
        _parameters.SearchString = searchString;
        var query = _queryBuilder.Execute(_parameters);
        var expected = _housings.Where(housing => housing.Name == searchString);
        
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