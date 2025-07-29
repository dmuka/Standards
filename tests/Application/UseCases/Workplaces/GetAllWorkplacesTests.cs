using Application.UseCases.Workplaces;
using Domain.Aggregates.Workplaces;
using Moq;

namespace Tests.Application.UseCases.Workplaces;

[TestFixture]
public class GetAllWorkplacesTests
{
    private readonly WorkplaceId _workplaceId1 = new (Guid.CreateVersion7());
    private readonly WorkplaceId _workplaceId2 = new (Guid.CreateVersion7());
    
    private readonly Guid _roomId1 = Guid.CreateVersion7();
    private readonly Guid _responsibleId1 = Guid.CreateVersion7();
    private readonly Guid _sectorId1 = Guid.CreateVersion7();
    private readonly string? _imagePath1 = null;
    
    private readonly Guid _roomId2 = Guid.CreateVersion7();
    private readonly Guid _responsibleId2 = Guid.CreateVersion7();
    private readonly Guid _sectorId2 = Guid.CreateVersion7();
    private readonly string? _imagePath2 = null;
    
    private Workplace _workplace1;
    private Workplace _workplace2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IWorkplaceRepository> _workplaceRepositoryMock;
    
    private GetAllWorkplaces.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _workplace1 = Workplace.Create(
            "Workplace name 1",
            "Workplace short name 1",
            _roomId1,
            _responsibleId1,
            _sectorId1,
            _workplaceId1,
            _imagePath1).Value;
        _workplace2 = Workplace.Create(
            "Workplace name 2",
            "Workplace short name 2",
            _roomId2,
            _responsibleId2,
            _sectorId2,
            _workplaceId2,
            _imagePath2).Value;

        _workplaceRepositoryMock = new Mock<IWorkplaceRepository>();
        _workplaceRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_workplace1, _workplace2]);
        
        _handler = new GetAllWorkplaces.QueryHandler(_workplaceRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllWorkplaces()
    {
        // Arrange
        var query = new GetAllWorkplaces.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Workplace>(h => h.Id == _workplace1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Workplace>(h => h.Id == _workplace2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoWorkplacesExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllWorkplaces.Query();
        _workplaceRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
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