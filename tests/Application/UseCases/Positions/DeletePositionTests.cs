using Application.Abstractions.Data;
using Application.UseCases.Positions;
using Domain.Aggregates.Positions;
using Moq;

namespace Tests.Application.UseCases.Positions;

[TestFixture]
public class DeletePositionTests
{
    private const string PositionNameValue = "Position name";
    private const string PositionShortNameValue = "Position short name";
    
    private readonly Guid _invalidPositionIdGuid = Guid.CreateVersion7();
    private PositionId _invalidPositionId;
    
    private readonly Guid _validPositionIdGuid = Guid.CreateVersion7();
    private PositionId _validPositionId;
    private readonly PositionId _positionId = new (Guid.CreateVersion7());

    private Position _position;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private Mock<IPositionRepository> _positionRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private DeletePosition.CommandHandler _handler;

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

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new DeletePosition.CommandHandler(_positionRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_PositionExists_DeletesPositionAndReturnsSuccess()
    {
        // Arrange
        var command = new DeletePosition.Command(_validPositionId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _positionRepositoryMock.Verify(repository => repository.ExistsAsync(_validPositionId, _cancellationToken), Times.Once);
            _positionRepositoryMock.Verify(repository => repository.GetByIdAsync(_validPositionId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_PositionDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeletePosition.Command(_invalidPositionId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(PositionErrors.NotFound(_invalidPositionId)));
        }
    }
}