using Application.UseCases.Positions;
using Domain.Aggregates.Positions;
using Moq;

namespace Tests.Application.UseCases.Positions;

[TestFixture]
public class GetAllPositionsTests
{
    private readonly PositionId _positionId1 = new (Guid.CreateVersion7());
    private readonly PositionId _positionId2 = new (Guid.CreateVersion7());
 
    private Position _position1;
    private Position _position2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IPositionRepository> _positionRepositoryMock;
    
    private GetAllPositions.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _position1 = Position.Create(
            "Position name 1",
            "Position short name 1",
            _positionId1,
            "").Value;
        _position2 = Position.Create(
            "Position name 2",
            "Position short name 2",
            _positionId2,
            "").Value;

        _positionRepositoryMock = new Mock<IPositionRepository>();
        _positionRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_position1, _position2]);
        
        _handler = new GetAllPositions.QueryHandler(_positionRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllPositions()
    {
        // Arrange
        var query = new GetAllPositions.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Position>(h => h.Id == _position1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Position>(h => h.Id == _position2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoPositionsExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllPositions.Query();
        _positionRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
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