using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Positions;
using Core.Results;
using Domain.Aggregates.Positions;
using Moq;

namespace Tests.Application.UseCases.Positions;

[TestFixture]
public class AddPositionTests
{
    private const string PositionNameValue = "Position name";
    private const string PositionShortNameValue = "Position short name";
    
    private readonly PositionId _positionId = new (Guid.CreateVersion7());
    
    private PositionDto2 _positionDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IPositionRepository> _positionRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddPosition.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _positionDto = new PositionDto2
        {
            PositionName = PositionNameValue,
            PositionShortName = PositionShortNameValue,
            Id = _positionId
        };
        
        _positionRepositoryMock = new Mock<IPositionRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddPosition.CommandHandler(_positionRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_PositionSuccessfullyAdded_ReturnsSuccessResult()
    {
        // Arrange
        var command = new AddPosition.Command(_positionDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _positionRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Position>(), _cancellationToken), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(_cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_PositionCreationFails_ReturnsZero()
    {
        // Arrange
        _positionDto.PositionName = "";
        var command = new AddPosition.Command(_positionDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
            Assert.That(result.Error.Description, Is.EqualTo("One or more validation errors occurred"));
        }
    }
}