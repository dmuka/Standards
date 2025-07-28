using Application.UseCases.Departments;
using Domain.Aggregates.Departments;
using Moq;

namespace Tests.Application.UseCases.Departments;

[TestFixture]
public class GetDepartmentByIdTests
{
    private const string DepartmentNameValue = "Department name";
    private const string DepartmentShortNameValue = "Department short name";
    
    private readonly Guid _invalidDepartmentIdGuid = Guid.CreateVersion7();
    private DepartmentId _invalidDepartmentId;
    
    private readonly Guid _validDepartmentIdGuid = Guid.CreateVersion7();
    private DepartmentId _validDepartmentId;

    private Department _department;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IDepartmentRepository> _departmentRepositoryMock;
    
    private GetDepartmentById.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validDepartmentId = new DepartmentId(_validDepartmentIdGuid);
        _invalidDepartmentId = new DepartmentId(_invalidDepartmentIdGuid);
        
        _department = Department.Create(
            DepartmentNameValue, 
            DepartmentShortNameValue,
            _validDepartmentId,
            "Comments").Value;        

        _departmentRepositoryMock = new Mock<IDepartmentRepository>();
        _departmentRepositoryMock.Setup(repository => repository.ExistsAsync(_validDepartmentId, _cancellationToken))
            .ReturnsAsync(true);
        _departmentRepositoryMock.Setup(repository => repository.GetByIdAsync(_validDepartmentId, _cancellationToken))
            .ReturnsAsync(_department);
        
        _handler = new GetDepartmentById.QueryHandler(_departmentRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_DepartmentExists_ReturnsDepartment()
    {
        // Arrange
        var query = new GetDepartmentById.Query(_validDepartmentId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_validDepartmentId));
            Assert.That(result.Value.Name.Value, Is.EqualTo(DepartmentNameValue));
            Assert.That(result.Value.ShortName.Value, Is.EqualTo(DepartmentShortNameValue));
        }
    }

    [Test]
    public async Task Handle_DepartmentDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetDepartmentById.Query(_invalidDepartmentId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(DepartmentErrors.NotFound(_invalidDepartmentId)));
        }
    }
}