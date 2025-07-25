using Application.Abstractions.Data;
using Application.Exceptions;
using Application.UseCases.Rooms.EventHandlers;
using Core.Results;
using Domain.Aggregates.Rooms;
using Domain.Aggregates.Sectors;
using Domain.Aggregates.Sectors.Events.Domain;
using Infrastructure.Exceptions.Enum;
using Moq;

namespace Tests.Application.UseCases.Rooms.EventHandlers
{
    [TestFixture]
    public class RoomAddedToSectorEventHandlerTests
    {
        private readonly SectorId _sectorId = new SectorId(Guid.CreateVersion7());
        private RoomAddedToSectorEvent _event;
        
        private readonly CancellationToken _cancellationToken = CancellationToken.None;
        
        private Mock<IRoomRepository> _roomRepositoryMock;
        private Mock<IUnitOfWork> _unitOfWorkMock;
        
        private RoomAddedToSectorEventHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _event = new RoomAddedToSectorEvent(_sectorId, new RoomId(Guid.NewGuid()));
            
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _handler = new RoomAddedToSectorEventHandler(
                _roomRepositoryMock.Object,
                _unitOfWorkMock.Object);
        }

        [Test]
        public void Handle_RoomNotFound_ThrowsStandardsException()
        {
            // Arrange
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(_event.RoomId, _cancellationToken))
                .ReturnsAsync((Room)null!);

            // Act & Assert
            var ex = Assert.ThrowsAsync<StandardsException>(() => _handler.Handle(_event, _cancellationToken));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.Error, Is.EqualTo(StatusCodeByError.InternalServerError));
                Assert.That(ex.Message, Does.Contain("not found"));
            }
        }

        [Test]
        public void Handle_ChangeSectorFails_ThrowsStandardsException()
        {
            // Arrange
            var room = Room.Create(5, 2, 5).Value;
            room.ChangeSector(_sectorId);
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(_event.RoomId, _cancellationToken))
                .ReturnsAsync(room);

            // Act & Assert
            var ex = Assert.ThrowsAsync<StandardsException>(() => _handler.Handle(_event, _cancellationToken));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(ex.Error, Is.EqualTo(StatusCodeByError.InternalServerError));
                Assert.That(ex.Message, Does.Contain("change sector"));
            }
        }

        [Test]
        public async Task Handle_SuccessfulOperation_CommitsTransaction()
        {
            // Arrange
            var room = Room.Create(5, 2, 5).Value;
            _roomRepositoryMock.Setup(repo => repo.GetByIdAsync(_event.RoomId, _cancellationToken))
                .ReturnsAsync(room);

            // Act
            await _handler.Handle(_event, _cancellationToken);

            // Assert
            _roomRepositoryMock.Verify(repo => repo.Update(room), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.CommitAsync(_cancellationToken), Times.Once);
        }
    }
}