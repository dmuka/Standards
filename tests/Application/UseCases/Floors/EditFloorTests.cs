using Application.UseCases.DTOs;
using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Application.UseCases.Floors;

[TestFixture]
public class EditFloorTests
{
    private FloorDto _floorDto;
    private Floor _floor;
    
    private ApplicationDbContext _dbContext;
    private Mock<IFloorUniqueness> _floorUniquenessMock;
    private EditFloor.CommandHandler _handler;

    [SetUp]
    public async Task Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;
        
        _floorDto = new FloorDto
        {
            FloorId = new FloorId(Guid.CreateVersion7()), 
            Number = 1, 
            HousingId = new HousingId(Guid.CreateVersion7())
        };

        _floor = Floor.Create(_floorDto.Number, _floorDto.HousingId, _floorDto.FloorId).Value;

        _dbContext = new ApplicationDbContext(options);
        await _dbContext.Floors.AddAsync(_floor);
        await _dbContext.SaveChangesAsync();
        
        _floorUniquenessMock = new Mock<IFloorUniqueness>();
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync(_floorDto.Number, _floorDto.HousingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        _handler = new EditFloor.CommandHandler(_dbContext, _floorUniquenessMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }

    [Test]
    public async Task Handle_FloorNotUnique_ReturnsFailure()
    {
        // Arrange
        var command = new EditFloor.Command(_floorDto);
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync(_floorDto.Number, _floorDto.HousingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.FloorAlreadyExistOrWrong));
        }
    }

    [Test]
    public async Task Handle_FloorEditFails_ReturnsZero()
    {
        // Arrange
        _floorDto.Number = 0;
        var command = new EditFloor.Command(_floorDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(FloorErrors.FloorAlreadyExistOrWrong.Code));
        }
    }

    [Test]
    public async Task Handle_FloorSuccessfullyEdited_ReturnsNumberOfChanges()
    {
        // Arrange
        var command = new EditFloor.Command(_floorDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.EqualTo(1));
    }
}