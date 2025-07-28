using Application.Abstractions.Data;
using Application.UseCases.Departments;
using Domain.Aggregates.Departments;
using Moq;

namespace Tests.Application.UseCases.Departments;

[TestFixture]
public class DeleteDepartmentTests
{
    private const string DepartmentNameValue = "Department name";
    private const string DepartmentShortNameValue = "Department short name";
    
    private readonly Guid _invalidDepartmentIdGuid = Guid.CreateVersion7();
    private DepartmentId _invalidDepartmentId;
    
    private readonly Guid _validDepartmentIdGuid = Guid.CreateVersion7();
    private DepartmentId _validDepartmentId;
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());

    private Department _department;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private Mock<IDepartmentRepository> _departmentRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private DeleteDepartment.CommandHandler _handler;

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

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new DeleteDepartment.CommandHandler(_departmentRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_DepartmentExists_DeletesDepartmentAndReturnsSuccess()
    {
        // Arrange
        var command = new DeleteDepartment.Command(_validDepartmentId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _departmentRepositoryMock.Verify(repository => repository.ExistsAsync(_validDepartmentId, _cancellationToken), Times.Once);
            _departmentRepositoryMock.Verify(repository => repository.GetByIdAsync(_validDepartmentId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_DepartmentDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteDepartment.Command(_invalidDepartmentId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(DepartmentErrors.NotFound(_invalidDepartmentId)));
        }
    }
}