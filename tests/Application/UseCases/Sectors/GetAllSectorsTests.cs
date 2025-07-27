using Application.UseCases.Sectors;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Sectors;
using Moq;

namespace Tests.Application.UseCases.Sectors;

[TestFixture]
public class GetAllSectorsTests
{
    private readonly SectorId _sectorId1 = new (Guid.CreateVersion7());
    private readonly SectorId _sectorId2 = new (Guid.CreateVersion7());
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());
 
    private Sector _sector1;
    private Sector _sector2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<ISectorRepository> _sectorRepositoryMock;
    
    private GetAllSectors.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _sector1 = Sector.Create(
            "Sector name 1",
            "Sector short name 1",
            _sectorId1,
            _departmentId).Value;
        _sector2 = Sector.Create(
            "Sector name 2",
            "Sector short name 2",
            _sectorId2,
            _departmentId).Value;

        _sectorRepositoryMock = new Mock<ISectorRepository>();
        _sectorRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_sector1, _sector2]);
        
        _handler = new GetAllSectors.QueryHandler(_sectorRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllSectors()
    {
        // Arrange
        var query = new GetAllSectors.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Sector>(h => h.Id == _sector1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Sector>(h => h.Id == _sector2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoSectorsExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllSectors.Query();
        _sectorRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
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