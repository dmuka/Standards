using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Application.UseCases.Floors;

[TestFixture]
public class AddFloorTests
{
    private FloorDto _floorDto;
    private Mock<IFloorRepository> _floorRepositoryMock;
    private Mock<IFloorUniqueness> _floorUniquenessMock;
    private NullLogger<AddFloor> _loggerMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddFloor.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _floorRepositoryMock = new Mock<IFloorRepository>();
        
        _floorDto = new FloorDto
        {
            FloorId = new FloorId(Guid.CreateVersion7()), 
            Number = 1, 
            HousingId = new HousingId(Guid.CreateVersion7())
        };
        
        _floorUniquenessMock = new Mock<IFloorUniqueness>();
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync(_floorDto.Number, _floorDto.HousingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _loggerMock = NullLogger<AddFloor>.Instance;
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddFloor.CommandHandler(_floorRepositoryMock.Object, _floorUniquenessMock.Object, _unitOfWorkMock.Object, _loggerMock);
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
            Assert.That(result.Error, Is.EqualTo(FloorErrors.FloorAlreadyExistOrWrong));
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
            Assert.That(result.Error.Code, Is.EqualTo(FloorErrors.FloorAlreadyExistOrWrong.Code));
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
        Assert.That(result.IsSuccess, Is.True);
    }
}