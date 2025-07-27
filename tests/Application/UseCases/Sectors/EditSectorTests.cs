using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Sectors;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Sectors;
using Moq;

namespace Tests.Application.UseCases.Sectors;

[TestFixture]
public class EditSectorTests
{
    private readonly SectorId _sectorId = new (Guid.CreateVersion7());
    private readonly SectorId _nonExistentSectorId = new (Guid.CreateVersion7());
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());
    
    private SectorDto2 _sectorDto;
    private Sector _sector;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<ISectorRepository> _sectorRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditSector.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _sectorDto = new SectorDto2
        {
            SectorName = "Sector name",
            SectorShortName = "Sector short name",
            Id = _sectorId,
            DepartmentId = _departmentId
        };

        _sector = Sector.Create(
            _sectorDto.SectorName,
            _sectorDto.SectorShortName,
            _sectorId,
            _departmentId,
            "Comments").Value;

        _sectorRepositoryMock = new Mock<ISectorRepository>();
        _sectorRepositoryMock.Setup(repository => repository.ExistsAsync(_sectorId, _cancellationToken))
            .ReturnsAsync(true);
        _sectorRepositoryMock.Setup(repository => repository.GetByIdAsync(_sectorId, _cancellationToken))
            .ReturnsAsync(_sector);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditSector.CommandHandler(_sectorRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_SectorIdNotExist_ReturnsFailure()
    {
        // Arrange
        _sectorDto.Id = _nonExistentSectorId; 
        var command = new EditSector.Command(_sectorDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(SectorErrors.NotFound(_nonExistentSectorId).Code));
        }
    }

    [Test]
    public async Task Handle_SectorSuccessfullyEdited_ReturnsSuccessResult()
    {
        // Arrange
        var command = new EditSector.Command(_sectorDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}