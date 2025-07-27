using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Sectors;
using Core.Results;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Sectors;
using Moq;

namespace Tests.Application.UseCases.Sectors;

[TestFixture]
public class AddSectorTests
{
    private const string SectorNameValue = "Sector name";
    private const string SectorShortNameValue = "Sector short name";
    
    private readonly SectorId _sectorId = new (Guid.CreateVersion7());
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());
    
    private SectorDto2 _sectorDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<ISectorRepository> _sectorRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddSector.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _sectorDto = new SectorDto2
        {
            SectorName = SectorNameValue,
            SectorShortName = SectorShortNameValue,
            Id = _sectorId, 
            DepartmentId = _departmentId
        };
        
        _sectorRepositoryMock = new Mock<ISectorRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddSector.CommandHandler(_sectorRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_SectorSuccessfullyAdded_ReturnsSuccessResult()
    {
        // Arrange
        var command = new AddSector.Command(_sectorDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _sectorRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Sector>(), _cancellationToken), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(_cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_SectorCreationFails_ReturnsZero()
    {
        // Arrange
        _sectorDto.SectorName = "";
        var command = new AddSector.Command(_sectorDto);

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