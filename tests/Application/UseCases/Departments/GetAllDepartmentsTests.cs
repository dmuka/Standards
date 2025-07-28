using Application.UseCases.Departments;
using Domain.Aggregates.Departments;
using Moq;

namespace Tests.Application.UseCases.Departments;

[TestFixture]
public class GetAllDepartmentsTests
{
    private readonly DepartmentId _departmentId1 = new (Guid.CreateVersion7());
    private readonly DepartmentId _departmentId2 = new (Guid.CreateVersion7());
 
    private Department _department1;
    private Department _department2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IDepartmentRepository> _departmentRepositoryMock;
    
    private GetAllDepartments.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _department1 = Department.Create(
            "Department name 1",
            "Department short name 1",
            _departmentId1,
            "").Value;
        _department2 = Department.Create(
            "Department name 2",
            "Department short name 2",
            _departmentId2,
            "").Value;

        _departmentRepositoryMock = new Mock<IDepartmentRepository>();
        _departmentRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_department1, _department2]);
        
        _handler = new GetAllDepartments.QueryHandler(_departmentRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllDepartments()
    {
        // Arrange
        var query = new GetAllDepartments.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Department>(h => h.Id == _department1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Department>(h => h.Id == _department2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoDepartmentsExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllDepartments.Query();
        _departmentRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
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