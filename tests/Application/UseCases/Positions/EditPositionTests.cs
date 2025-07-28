using Application.Abstractions.Data;
using Application.UseCases.Positions;
using Application.UseCases.DTOs;
using Domain.Aggregates.Positions;
using Moq;

namespace Tests.Application.UseCases.Positions;

[TestFixture]
public class EditPositionTests
{
    private readonly PositionId _positionId = new (Guid.CreateVersion7());
    private readonly PositionId _nonExistentPositionId = new (Guid.CreateVersion7());
    
    private PositionDto2 _positionDto;
    private Position _position;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IPositionRepository> _positionRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditPosition.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _positionDto = new PositionDto2
        {
            PositionName = "Position name",
            PositionShortName = "Position short name",
            Id = _positionId
        };

        _position = Position.Create(
            _positionDto.PositionName,
            _positionDto.PositionShortName,
            _positionId,
            "Comments").Value;

        _positionRepositoryMock = new Mock<IPositionRepository>();
        _positionRepositoryMock.Setup(repository => repository.ExistsAsync(_positionId, _cancellationToken))
            .ReturnsAsync(true);
        _positionRepositoryMock.Setup(repository => repository.GetByIdAsync(_positionId, _cancellationToken))
            .ReturnsAsync(_position);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditPosition.CommandHandler(_positionRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_PositionIdNotExist_ReturnsFailure()
    {
        // Arrange
        _positionDto.Id = _nonExistentPositionId; 
        var command = new EditPosition.Command(_positionDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(PositionErrors.NotFound(_nonExistentPositionId).Code));
        }
    }

    [Test]
    public async Task Handle_PositionSuccessfullyEdited_ReturnsSuccessResult()
    {
        // Arrange
        var command = new EditPosition.Command(_positionDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}