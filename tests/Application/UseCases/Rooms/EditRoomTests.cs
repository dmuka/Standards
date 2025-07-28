using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Domain.Services;
using Moq;

namespace Tests.Application.UseCases.Rooms;

[TestFixture]
public class EditRoomTests
{
    private const int FloorNumber = 1;
    
    private static readonly Guid ValidFloorIdGuid = Guid.CreateVersion7();
    private readonly FloorId _validFloorId = new (ValidFloorIdGuid);    
    
    private static readonly Guid InvalidFloorIdGuid = Guid.CreateVersion7();
    private readonly FloorId _invalidFloorId = new (InvalidFloorIdGuid);
    
    private static readonly Guid ValidHousingIdGuid = Guid.CreateVersion7();
    private readonly HousingId _validHousingId = new HousingId(ValidHousingIdGuid);

    private Floor _floor;
    private FloorDto _floorDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    private Mock<IFloorRepository> _floorRepositoryMock;
    private Mock<IChildEntityUniqueness> _floorUniquenessMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditFloor.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _floorDto = new FloorDto
        {
            Id = _validFloorId, 
            Number = FloorNumber, 
            HousingId = _validHousingId
        };

        _floor = Floor.Create(_floorDto.Number, _floorDto.HousingId, _floorDto.Id).Value;

        _floorRepositoryMock = new Mock<IFloorRepository>();
        _floorRepositoryMock.Setup(r => r.ExistsAsync(_validFloorId, _cancellationToken))
            .ReturnsAsync(true);
        _floorRepositoryMock.Setup(r => r.GetByIdAsync(_validFloorId, _cancellationToken))
            .ReturnsAsync(_floor);
        
        _floorUniquenessMock = new Mock<IChildEntityUniqueness>();
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync<FloorDto, HousingDto2>(_floorDto.Id, _floorDto.HousingId, _cancellationToken))
            .ReturnsAsync(true);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditFloor.CommandHandler(_floorRepositoryMock.Object, _floorUniquenessMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_FloorNotUnique_ReturnsFailure()
    {
        // Arrange
        var command = new EditFloor.Command(_floorDto);
        _floorUniquenessMock.Setup(x => x.IsUniqueAsync<FloorDto, HousingDto2>(_floorDto.Id, _floorDto.HousingId, _cancellationToken))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.FloorAlreadyExistOrWrong));
            _floorRepositoryMock.Verify(repository => repository.ExistsAsync(_validFloorId, _cancellationToken), Times.Once);
            _floorRepositoryMock.Verify(repository => repository.GetByIdAsync(_validFloorId, _cancellationToken), Times.Never);
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
        Assert.That(result.IsSuccess, Is.True);
        _floorRepositoryMock.Verify(repository => repository.ExistsAsync(_validFloorId, _cancellationToken), Times.Once);
        _floorRepositoryMock.Verify(repository => repository.GetByIdAsync(_validFloorId, _cancellationToken), Times.Once);
        _floorRepositoryMock.Verify(repository => repository.Update(_floor), Times.Once);
    }
}