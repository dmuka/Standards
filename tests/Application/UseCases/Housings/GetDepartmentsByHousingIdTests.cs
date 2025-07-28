using System.Linq.Expressions;
using Application.UseCases.Housings;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Housings;
using Domain.Aggregates.Sectors;
using Microsoft.EntityFrameworkCore;
using MockQueryable;
using Moq;

namespace Tests.Application.UseCases.Housings;

[TestFixture]
public class GetDepartmentsByHousingIdTests
{
    private readonly HousingId _housingId = new (Guid.CreateVersion7());
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());

    private Department _department;
    private Sector _sector;
        
    private Mock<IHousingRepository> _housingRepositoryMock;
    private Mock<IDepartmentRepository> _departmentRepositoryMock;
    private Mock<ISectorRepository> _sectorRepositoryMock;
        
    private GetDepartmentsByHousingId.QueryHandler _queryHandler;

    [SetUp]
    public void SetUp()
    {
        _sector = Sector.Create("Sector name", "Sector short name", departmentId: _departmentId).Value;
        _department = Department.Create("Department name", "Department short name", _departmentId).Value;
            
        _housingRepositoryMock = new Mock<IHousingRepository>();
        _housingRepositoryMock.Setup(repo => repo.ExistsAsync(_housingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);
            
        _departmentRepositoryMock = new Mock<IDepartmentRepository>();
        _sectorRepositoryMock = new Mock<ISectorRepository>();

        _queryHandler = new GetDepartmentsByHousingId.QueryHandler(
            _housingRepositoryMock.Object,
            _departmentRepositoryMock.Object,
            _sectorRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_HousingDoesNotExist_ReturnsFailure()
    {
        // Arrange
        _housingRepositoryMock.Setup(repo => repo.ExistsAsync(_housingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var query = new GetDepartmentsByHousingId.Query(_housingId);

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(HousingErrors.NotFound(_housingId)));
        }
    }

    [Test]
    public async Task Handle_NoDepartments_ReturnsEmptyList()
    {
        // Arrange
        _sectorRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        var query = new GetDepartmentsByHousingId.Query(_housingId);

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Is.Empty);
        }
    }

    [Test]
    public async Task Handle_DepartmentsExist_ReturnsDepartments()
    {
        // Arrange
        var departments = new List<Department> { _department };
        var departmentsMock = departments.BuildMock();
        _sectorRepositoryMock.Setup(repo => repo.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([_sector]);
        _departmentRepositoryMock.Setup(repo => repo.Where(It.IsAny<Expression<Func<Department, bool>>>()))
            .Returns(departmentsMock);

        var query = new GetDepartmentsByHousingId.Query(_housingId);

        // Act
        var result = await _queryHandler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value, Has.Count.EqualTo(1));
            Assert.That(result.Value.First().Id, Is.EqualTo(_departmentId));
        }
    }
}