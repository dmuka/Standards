using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Rooms;
using Core.Results;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Housings;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Tests.Application.UseCases.Rooms;

[TestFixture]
public class AddRoomTests
{
    private RoomDto2 _roomDto;
    private Mock<IRoomRepository> _roomRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddRoom.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _roomRepositoryMock = new Mock<IRoomRepository>();
        
        _roomDto = new RoomDto2
        {
            Id = new RoomId(Guid.CreateVersion7()), 
            Length = 5,  
            Width = 5,  
            Height = 2, 
            FloorId = new FloorId(Guid.CreateVersion7()),
            HousingId = new HousingId(Guid.CreateVersion7())
        };
        
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddRoom.CommandHandler(_roomRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_RoomCreationFails_ReturnsZero()
    {
        // Arrange
        _roomDto.Height = 0;
        var command = new AddRoom.Command(_roomDto);

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
    public async Task Handle_RoomSuccessfullyAdded_ReturnsNumberOfChanges()
    {
        // Arrange
        var command = new AddRoom.Command(_roomDto);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}