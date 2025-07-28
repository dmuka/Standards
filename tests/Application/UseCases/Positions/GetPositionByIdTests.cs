using Application.UseCases.Positions;
using Domain.Aggregates.Positions;
using Moq;

namespace Tests.Application.UseCases.Positions;

[TestFixture]
public class GetPositionByIdTests
{
    private const string PositionNameValue = "Position name";
    private const string PositionShortNameValue = "Position short name";
    
    private readonly Guid _invalidPositionIdGuid = Guid.CreateVersion7();
    private PositionId _invalidPositionId;
    
    private readonly Guid _validPositionIdGuid = Guid.CreateVersion7();
    private PositionId _validPositionId;

    private Position _position;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IPositionRepository> _positionRepositoryMock;
    
    private GetPositionById.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validPositionId = new PositionId(_validPositionIdGuid);
        _invalidPositionId = new PositionId(_invalidPositionIdGuid);
        
        _position = Position.Create(
            PositionNameValue, 
            PositionShortNameValue,
            _validPositionId,
            "Comments").Value;        

        _positionRepositoryMock = new Mock<IPositionRepository>();
        _positionRepositoryMock.Setup(repository => repository.ExistsAsync(_validPositionId, _cancellationToken))
            .ReturnsAsync(true);
        _positionRepositoryMock.Setup(repository => repository.GetByIdAsync(_validPositionId, _cancellationToken))
            .ReturnsAsync(_position);
        
        _handler = new GetPositionById.QueryHandler(_positionRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_PositionExists_ReturnsPosition()
    {
        // Arrange
        var query = new GetPositionById.Query(_validPositionId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_validPositionId));
            Assert.That(result.Value.Name.Value, Is.EqualTo(PositionNameValue));
            Assert.That(result.Value.ShortName.Value, Is.EqualTo(PositionShortNameValue));
        }
    }

    [Test]
    public async Task Handle_PositionDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetPositionById.Query(_invalidPositionId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(PositionErrors.NotFound(_invalidPositionId)));
        }
    }
}