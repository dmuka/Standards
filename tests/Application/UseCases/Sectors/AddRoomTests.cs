using Application.Abstractions.Data;
using Application.UseCases.Sectors;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;
using Moq;

namespace Tests.Application.UseCases.Sectors;

[TestFixture]
public class AddRoomTests
{
    private readonly Guid _roomIdValue = Guid.CreateVersion7();
    private readonly Guid _sectorIdValue = Guid.CreateVersion7();
    
    private static readonly SectorId AnotherSectorId = new (Guid.CreateVersion7());

    private RoomId _roomId;
    private SectorId _sectorId;

    private Sector _sector;
    private Room _room;
        
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    private AddRoom.Command _command;
        
    private Mock<ISectorRepository> _sectorRepositoryMock;
    private Mock<IRoomRepository> _roomRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddRoom.CommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _roomId = new RoomId(_roomIdValue);
        _sectorId = new SectorId(_sectorIdValue);
            
        _sector = Sector.Create(
            "Sector name", 
            "Sector short name",  
            _sectorIdValue).Value;
        _room = Room.Create(5, 2, 5, _roomId).Value;
            
        _command = new AddRoom.Command(_sectorId, _roomId);
            
        _sectorRepositoryMock = new Mock<ISectorRepository>();
        _sectorRepositoryMock.Setup(repo => repo.GetByIdAsync(_sectorId, _cancellationToken))
            .ReturnsAsync(_sector);
            
        _roomRepositoryMock = new Mock<IRoomRepository>();
        _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(_roomId, _cancellationToken))
            .ReturnsAsync(_room);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
            
        _handler = new AddRoom.CommandHandler(_sectorRepositoryMock.Object, _roomRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_SectorNotFound_ReturnsFailure()
    {
        // Arrange
        _sectorRepositoryMock.Setup(repo => repo.GetByIdAsync(_sectorId, _cancellationToken))
            .ReturnsAsync((Sector)null!);

        // Act
        var result = await _handler.Handle(_command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(SectorErrors.NotFound(_command.SectorId)));
        }
    }

    [Test]
    public async Task Handle_RoomNotFound_ReturnsFailure()
    {
        // Arrange
        _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(_roomId, _cancellationToken))
            .ReturnsAsync((Room)null!);

        // Act
        var result = await _handler.Handle(_command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(RoomErrors.NotFound(_command.RoomId)));
        }
    }

    [Test]
    public async Task Handle_RoomAlreadyBelongsToAnotherSector_ReturnsFailure()
    {
        // Arrange
        _room.ChangeSector(AnotherSectorId);

        // Act
        var result = await _handler.Handle(_command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(SectorErrors.ThisRoomAlreadySetForAnotherSector));
        }
    }

    [Test]
    public async Task Handle_ValidRequest_ReturnsSuccess()
    {
        // Arrange & Act
        var result = await _handler.Handle(_command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}