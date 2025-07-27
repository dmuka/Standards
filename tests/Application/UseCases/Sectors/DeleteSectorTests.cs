using Application.Abstractions.Data;
using Application.UseCases.Sectors;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Sectors;
using Moq;

namespace Tests.Application.UseCases.Sectors;

[TestFixture]
public class DeleteSectorTests
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
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private DeleteSector.CommandHandler _handler;

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

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new DeleteSector.CommandHandler(_sectorRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_SectorExists_DeletesSectorAndReturnsSuccess()
    {
        // Arrange
        var command = new DeleteSector.Command(_validSectorId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _sectorRepositoryMock.Verify(repository => repository.ExistsAsync(_validSectorId, _cancellationToken), Times.Once);
            _sectorRepositoryMock.Verify(repository => repository.GetByIdAsync(_validSectorId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_SectorDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteSector.Command(_invalidSectorId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(SectorErrors.NotFound(_invalidSectorId)));
        }
    }
}