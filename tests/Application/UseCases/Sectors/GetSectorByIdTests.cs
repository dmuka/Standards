using Application.UseCases.Sectors;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Sectors;
using Moq;

namespace Tests.Application.UseCases.Sectors;

[TestFixture]
public class GetSectorByIdTests
{
    private const string SectorNameValue = "Sector name";
    private const string SectorShortNameValue = "Sector short name";
    
    private readonly Guid _invalidSectorIdGuid = Guid.CreateVersion7();
    private SectorId _invalidSectorId;
    
    private readonly Guid _validSectorIdGuid = Guid.CreateVersion7();
    private SectorId _validSectorId;
    
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());

    private Sector _sector;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<ISectorRepository> _sectorRepositoryMock;
    
    private GetSectorById.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validSectorId = new SectorId(_validSectorIdGuid);
        _invalidSectorId = new SectorId(_invalidSectorIdGuid);
        
        _sector = Sector.Create(
            SectorNameValue, 
            SectorShortNameValue,
            _validSectorId,
            _departmentId,
            "Comments").Value;        

        _sectorRepositoryMock = new Mock<ISectorRepository>();
        _sectorRepositoryMock.Setup(repository => repository.ExistsAsync(_validSectorId, _cancellationToken))
            .ReturnsAsync(true);
        _sectorRepositoryMock.Setup(repository => repository.GetByIdAsync(_validSectorId, _cancellationToken))
            .ReturnsAsync(_sector);
        
        _handler = new GetSectorById.QueryHandler(_sectorRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_SectorExists_ReturnsSector()
    {
        // Arrange
        var query = new GetSectorById.Query(_validSectorId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_validSectorId));
            Assert.That(result.Value.Name.Value, Is.EqualTo(SectorNameValue));
            Assert.That(result.Value.ShortName.Value, Is.EqualTo(SectorShortNameValue));
        }
    }

    [Test]
    public async Task Handle_SectorDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetSectorById.Query(_invalidSectorId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(SectorErrors.NotFound(_invalidSectorId)));
        }
    }
}