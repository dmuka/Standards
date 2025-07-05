using Application.UseCases.Housings;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class GetAllHousingsTests
{
    private readonly HousingId _housingId1 = new (Guid.CreateVersion7());
    private readonly HousingId _housingId2 = new (Guid.CreateVersion7());
 
    private Housing _housing1;
    private Housing _housing2;
    
    private ApplicationDbContext _dbContext;
    private GetAllHousings.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _housing1 = Housing.Create(
            HousingName.Create("Housing name 1").Value,
            HousingShortName.Create("Housing short name 1").Value,
            Address.Create("Housing address line 1").Value,
            _housingId1).Value;
        _housing2 = Housing.Create(
            HousingName.Create("Housing name 2").Value,
            HousingShortName.Create("Housing short name 2").Value,
            Address.Create("Housing address line 2").Value,
            _housingId2).Value;
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _handler = new GetAllHousings.QueryHandler(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllHousings()
    {
        // Arrange
        await _dbContext.Housings2.AddRangeAsync(_housing1, _housing2);
        await _dbContext.SaveChangesAsync();

        var query = new GetAllHousings.Query();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Housing>(h => h.Id == _housing1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Housing>(h => h.Id == _housing2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoHousingsExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllHousings.Query();

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