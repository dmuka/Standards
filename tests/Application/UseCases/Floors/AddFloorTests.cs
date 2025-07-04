using Application.UseCases.DTOs;
using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests.Application.UseCases.Floors;

[TestFixture]
public class AddFloorTests
{
    private FloorDto _floorDto;
    private ApplicationDbContext _dbContext;
    private Mock<IFloorUniqueness> _floorUniquenessMock;
    private AddFloor.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _dbContext = new ApplicationDbContext(options);
        
        _floorDto = new FloorDto
        {
            FloorId = new FloorId(Guid.CreateVersion7()), 
            Number = 1, 
            HousingId = new HousingId(Guid.CreateVersion7())
        };
        
        _floorUniquenessMock = new Mock<IFloorUniqueness>();
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync(_floorDto.Number, _floorDto.HousingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
        
        _handler = new AddFloor.CommandHandler(_dbContext, _floorUniquenessMock.Object);
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
        var command = new AddFloor.Command(_floorDto);
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync(_floorDto.Number, _floorDto.HousingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.FloorAlreadyExist));
        }
    }

    [Test]
    public async Task Handle_FloorCreationFails_ReturnsZero()
    {
        // Arrange
        _floorDto.Number = 0;
        var command = new AddFloor.Command(_floorDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(FloorErrors.FloorAlreadyExist.Code));
        }
    }

    [Test]
    public async Task Handle_FloorSuccessfullyAdded_ReturnsNumberOfChanges()
    {
        // Arrange
        var command = new AddFloor.Command(_floorDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.Value, Is.EqualTo(1));
    }
}