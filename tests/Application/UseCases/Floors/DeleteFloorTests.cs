using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Moq;

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

    private Floor _floor;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    private Mock<IFloorRepository> _floorRepositoryMock;
    
    private DeleteFloor.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validFloorId = new FloorId(_validFloorIdGuid);
        _validHousingId = new HousingId(_validHousingIdGuid);

        _invalidFloorId = new FloorId(_invalidFloorIdGuid);
        
        _floor = Floor.Create(FloorNumber, _validHousingId, _validFloorId).Value;
        
        _floorRepositoryMock = new Mock<IFloorRepository>();
        _floorRepositoryMock.Setup(r => r.ExistsAsync(_validFloorId, _cancellationToken))
            .ReturnsAsync(true);
        _floorRepositoryMock.Setup(r => r.GetByIdAsync(_validFloorId, _cancellationToken))
            .ReturnsAsync(_floor);
        
        _handler = new DeleteFloor.CommandHandler(_floorRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_FloorExists_DeletesFloorAndReturnsSuccess()
    {
        // Arrange
        var command = new DeleteFloor.Command(_validFloorId);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _floorRepositoryMock.Verify(repository => repository.ExistsAsync(_validFloorId, _cancellationToken), Times.Once);
            _floorRepositoryMock.Verify(repository => repository.GetByIdAsync(_validFloorId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_FloorDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteFloor.Command(_invalidFloorId);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(FloorErrors.NotFound(_invalidFloorId)));
            _floorRepositoryMock.Verify(repository => repository.ExistsAsync(_invalidFloorId, _cancellationToken), Times.Once);
            _floorRepositoryMock.Verify(repository => repository.GetByIdAsync(_invalidFloorId, _cancellationToken), Times.Never);
        }
    }
}