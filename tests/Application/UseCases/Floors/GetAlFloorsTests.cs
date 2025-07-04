using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.Application.UseCases.Floors;

[TestFixture]
public class GetAllFloorsTests
{
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    private readonly FloorId _floorId1 = new (Guid.CreateVersion7());
    private readonly FloorId _floorId2 = new (Guid.CreateVersion7());
 
    private Floor _floor1;
    private Floor _floor2;
    
    private ApplicationDbContext _dbContext;
    private GetAllFloors.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _floor1 = Floor.Create(1, _housingId, _floorId1).Value;
        _floor2 = Floor.Create(2, _housingId, _floorId2).Value;
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _handler = new GetAllFloors.QueryHandler(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllFloors()
    {
        // Arrange
        await _dbContext.Floors.AddRangeAsync(_floor1, _floor2);
        await _dbContext.SaveChangesAsync();

        var query = new GetAllFloors.Query();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Floor>(f => f.Number == _floor1.Number));
            Assert.That(result, Has.Exactly(1).Matches<Floor>(f => f.Number == _floor2.Number));
        }
    }

    [Test]
    public async Task Handle_WhenNoFloorsExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllFloors.Query();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }
    }
}