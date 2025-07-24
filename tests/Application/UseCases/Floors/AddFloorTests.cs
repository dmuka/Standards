using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Floors;
using Confluent.Kafka;
using Core.Results;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Domain.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Application.UseCases.Floors;

[TestFixture]
public class AddFloorTests
{
    private FloorDto _floorDto;
    private Mock<IFloorRepository> _floorRepositoryMock;
    private Mock<IChildEntityUniqueness> _floorUniquenessMock;
    private NullLogger<AddFloor> _loggerMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddFloor.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _floorRepositoryMock = new Mock<IFloorRepository>();
        
        _floorDto = new FloorDto
        {
            Id = new FloorId(Guid.CreateVersion7()), 
            Number = 1, 
            HousingId = new HousingId(Guid.CreateVersion7())
        };
        
        _floorUniquenessMock = new Mock<IChildEntityUniqueness>();
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync<FloorDto, HousingDto2>(_floorDto.Id, _floorDto.HousingId, It.IsAny<CancellationToken>()))
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
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync<FloorDto, HousingDto2>(_floorDto.Id, _floorDto.HousingId, It.IsAny<CancellationToken>()))
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
            Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
            Assert.That(result.Error.Description, Is.EqualTo("One or more validation errors occurred"));
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