using Application.Abstractions.Data;
using Application.UseCases.Workplaces;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Workplaces;
using Moq;

namespace Tests.Application.UseCases.Workplaces;

[TestFixture]
public class DeleteWorkplaceTests
{
    private const string WorkplaceNameValue = "Workplace name";
    private const string WorkplaceShortNameValue = "Workplace short name";
    
    private readonly Guid _invalidWorkplaceIdGuid = Guid.CreateVersion7();
    private WorkplaceId _invalidWorkplaceId;
    
    private readonly Guid _validWorkplaceIdGuid = Guid.CreateVersion7();
    private WorkplaceId _validWorkplaceId;
    private readonly Guid _roomId = Guid.CreateVersion7();
    private readonly Guid _responsibleId = Guid.CreateVersion7();
    private readonly Guid _sectorId = Guid.CreateVersion7();
    private readonly string? _imagePath = null;
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());

    private Workplace _workplace;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private Mock<IWorkplaceRepository> _workplaceRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private DeleteWorkplace.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validWorkplaceId = new WorkplaceId(_validWorkplaceIdGuid);
        _invalidWorkplaceId = new WorkplaceId(_invalidWorkplaceIdGuid);
        
        _workplace = Workplace.Create(
                               WorkplaceNameValue, 
                               WorkplaceShortNameValue,
                               _roomId,
                               _responsibleId,
                               _sectorId,
                               _validWorkplaceId,
                               _imagePath,
                               "Comments").Value;
        
        _workplaceRepositoryMock = new Mock<IWorkplaceRepository>();
        _workplaceRepositoryMock.Setup(repository => repository.ExistsAsync(_validWorkplaceId, _cancellationToken))
            .ReturnsAsync(true);
        _workplaceRepositoryMock.Setup(repository => repository.GetByIdAsync(_validWorkplaceId, _cancellationToken))
            .ReturnsAsync(_workplace);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new DeleteWorkplace.CommandHandler(_workplaceRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_WorkplaceExists_DeletesWorkplaceAndReturnsSuccess()
    {
        // Arrange
        var command = new DeleteWorkplace.Command(_validWorkplaceId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _workplaceRepositoryMock.Verify(repository => repository.ExistsAsync(_validWorkplaceId, _cancellationToken), Times.Once);
            _workplaceRepositoryMock.Verify(repository => repository.GetByIdAsync(_validWorkplaceId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_WorkplaceDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteWorkplace.Command(_invalidWorkplaceId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(WorkplaceErrors.NotFound(_invalidWorkplaceId)));
        }
    }
}