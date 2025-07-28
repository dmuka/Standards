using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Domain.Aggregates.Housings;
using Moq;

namespace Tests.Application.UseCases.Rooms;

[TestFixture]
public class GetAllFloorsTests
{
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    private readonly FloorId _floorId1 = new (Guid.CreateVersion7());
    private readonly FloorId _floorId2 = new (Guid.CreateVersion7());
 
    private Floor _floor1;
    private Floor _floor2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IFloorRepository> _floorRepositoryMock;
    
    private GetAllFloors.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _floor1 = Floor.Create(1, _housingId, _floorId1).Value;
        _floor2 = Floor.Create(2, _housingId, _floorId2).Value;

        _floorRepositoryMock = new Mock<IFloorRepository>();
        _floorRepositoryMock.Setup(r => r.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_floor1, _floor2]);
        
        _handler = new GetAllFloors.QueryHandler(_floorRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllFloors()
    {
        // Arrange
        var query = new GetAllFloors.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert.
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Floor>(f => f.Number == _floor1.Number));
            Assert.That(result, Has.Exactly(1).Matches<Floor>(f => f.Number == _floor2.Number));
        }
    }

    [Test]
    public async Task Handle_WhenNoFloorsExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllFloors.Query();
        _floorRepositoryMock.Setup(r => r.GetAllAsync(_cancellationToken))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}