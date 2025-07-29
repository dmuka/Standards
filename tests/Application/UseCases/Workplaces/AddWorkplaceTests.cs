using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Workplaces;
using Core.Results;
using Domain.Aggregates.Workplaces;
using Moq;

namespace Tests.Application.UseCases.Workplaces;

[TestFixture]
public class AddWorkplaceTests
{
    private const string WorkplaceNameValue = "Workplace name";
    private const string WorkplaceShortNameValue = "Workplace short name";
    
    private readonly Guid _workplaceId = Guid.CreateVersion7();
    private readonly Guid _roomId = Guid.CreateVersion7();
    private readonly Guid _responsibleId = Guid.CreateVersion7();
    private readonly Guid _sectorId = Guid.CreateVersion7();
    private readonly string? _imagePath = null;
    
    private WorkplaceDto2 _workplaceDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IWorkplaceRepository> _workplaceRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddWorkplace.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _workplaceDto = new WorkplaceDto2
        {
            WorkplaceName = WorkplaceNameValue,
            WorkplaceShortName = WorkplaceShortNameValue,
            Id = _workplaceId, 
            RoomId = _roomId,
            ResponsibleId = _responsibleId,
            SectorId = _sectorId,
            ImagePath = _imagePath
        };
        
        _workplaceRepositoryMock = new Mock<IWorkplaceRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddWorkplace.CommandHandler(_workplaceRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_WorkplaceSuccessfullyAdded_ReturnsSuccessResult()
    {
        // Arrange
        var command = new AddWorkplace.Command(_workplaceDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _workplaceRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Workplace>(), _cancellationToken), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(_cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_WorkplaceCreationFails_ReturnsZero()
    {
        // Arrange
        _workplaceDto.WorkplaceName = "";
        var command = new AddWorkplace.Command(_workplaceDto);

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