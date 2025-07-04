using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Tests.Application.UseCases.Floors;

[TestFixture]
public class DeleteFloorTests
{
    private const int FloorNumber = 1;
    
    private readonly Guid _validFloorIdGuid = Guid.CreateVersion7();
    private FloorId _validFloorId;    
    
    private readonly Guid _invalidFloorIdGuid = Guid.CreateVersion7();
    private FloorId _invalidFloorId;
    
    private readonly Guid _validHousingIdGuid = Guid.CreateVersion7();
    private HousingId _validHousingId;
    
    private ApplicationDbContext _dbContext;
    private DeleteFloor.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validFloorId = new FloorId(_validFloorIdGuid);
        _validHousingId = new HousingId(_validHousingIdGuid);

        _invalidFloorId = new FloorId(_invalidFloorIdGuid);
        
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        _handler = new DeleteFloor.CommandHandler(_dbContext);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_FloorExists_DeletesFloorAndReturnsSuccess()
    {
        // Arrange
        var floor = Floor.Create(FloorNumber, _validHousingId, _validFloorId).Value;
        await _dbContext.Floors.AddAsync(floor);
        await _dbContext.SaveChangesAsync();

        var command = new DeleteFloor.Command(_validFloorId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.EqualTo(1));
            Assert.That(await _dbContext.Floors.FindAsync(_validFloorId), Is.Null);
        }
    }

    [Test]
    public async Task Handle_FloorDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteFloor.Command(_invalidFloorId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.NotFound(_invalidFloorId)));
        }
    }
}