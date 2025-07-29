using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Workplaces;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Workplaces;
using Moq;

namespace Tests.Application.UseCases.Workplaces;

[TestFixture]
public class EditWorkplaceTests
{
    private readonly WorkplaceId _workplaceId = new (Guid.CreateVersion7());
    private readonly WorkplaceId _nonExistentWorkplaceId = new (Guid.CreateVersion7());
    
    private readonly Guid _roomId = Guid.CreateVersion7();
    private readonly Guid _responsibleId = Guid.CreateVersion7();
    private readonly Guid _sectorId = Guid.CreateVersion7();
    private readonly string? _imagePath = null;
    
    private WorkplaceDto2 _workplaceDto;
    private Workplace _workplace;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IWorkplaceRepository> _workplaceRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditWorkplace.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _workplaceDto = new WorkplaceDto2
        {
            WorkplaceName = "Workplace name",
            WorkplaceShortName = "Workplace short name",
            Id = _workplaceId,
            RoomId = _roomId,
            ResponsibleId = _responsibleId,
            SectorId = _sectorId,
            ImagePath = _imagePath,
            Comments = "Comments"
        };

        _workplace = Workplace.Create(
            _workplaceDto.WorkplaceName,
            _workplaceDto.WorkplaceShortName,
            _roomId,
            _responsibleId,
            _sectorId,
            _workplaceId,
            _imagePath,
            "Comments").Value;

        _workplaceRepositoryMock = new Mock<IWorkplaceRepository>();
        _workplaceRepositoryMock.Setup(repository => repository.ExistsAsync(_workplaceId, _cancellationToken))
            .ReturnsAsync(true);
        _workplaceRepositoryMock.Setup(repository => repository.GetByIdAsync(_workplaceId, _cancellationToken))
            .ReturnsAsync(_workplace);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditWorkplace.CommandHandler(_workplaceRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_WorkplaceIdNotExist_ReturnsFailure()
    {
        // Arrange
        _workplaceDto.Id = _nonExistentWorkplaceId; 
        var command = new EditWorkplace.Command(_workplaceDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(WorkplaceErrors.NotFound(_nonExistentWorkplaceId).Code));
        }
    }

    [Test]
    public async Task Handle_WorkplaceSuccessfullyEdited_ReturnsSuccessResult()
    {
        // Arrange
        var command = new EditWorkplace.Command(_workplaceDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}