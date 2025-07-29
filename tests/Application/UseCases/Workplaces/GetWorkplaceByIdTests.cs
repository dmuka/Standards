using Application.UseCases.Workplaces;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Workplaces;
using Moq;

namespace Tests.Application.UseCases.Workplaces;

[TestFixture]
public class GetWorkplaceByIdTests
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

    private Workplace _sector;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IWorkplaceRepository> _sectorRepositoryMock;
    
    private GetWorkplaceById.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validWorkplaceId = new WorkplaceId(_validWorkplaceIdGuid);
        _invalidWorkplaceId = new WorkplaceId(_invalidWorkplaceIdGuid);
        
        _sector = Workplace.Create(
            WorkplaceNameValue, 
            WorkplaceShortNameValue,
            _roomId,
            _responsibleId,
            _sectorId,
            _validWorkplaceId,
            _imagePath,
            "Comments").Value;        

        _sectorRepositoryMock = new Mock<IWorkplaceRepository>();
        _sectorRepositoryMock.Setup(repository => repository.ExistsAsync(_validWorkplaceId, _cancellationToken))
            .ReturnsAsync(true);
        _sectorRepositoryMock.Setup(repository => repository.GetByIdAsync(_validWorkplaceId, _cancellationToken))
            .ReturnsAsync(_sector);
        
        _handler = new GetWorkplaceById.QueryHandler(_sectorRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WorkplaceExists_ReturnsWorkplace()
    {
        // Arrange
        var query = new GetWorkplaceById.Query(_validWorkplaceId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_validWorkplaceId));
            Assert.That(result.Value.Name.Value, Is.EqualTo(WorkplaceNameValue));
            Assert.That(result.Value.ShortName.Value, Is.EqualTo(WorkplaceShortNameValue));
        }
    }

    [Test]
    public async Task Handle_WorkplaceDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetWorkplaceById.Query(_invalidWorkplaceId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(WorkplaceErrors.NotFound(_invalidWorkplaceId)));
        }
    }
}