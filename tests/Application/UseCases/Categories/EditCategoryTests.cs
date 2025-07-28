using Application.Abstractions.Data;
using Application.UseCases.Departments;
using Application.UseCases.DTOs;
using Domain.Aggregates.Departments;
using Moq;

namespace Tests.Application.UseCases.Categories;

[TestFixture]
public class EditCategoryTests
{
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());
    private readonly DepartmentId _nonExistentDepartmentId = new (Guid.CreateVersion7());
    
    private DepartmentDto2 _departmentDto;
    private Department _department;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IDepartmentRepository> _departmentRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditDepartment.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _departmentDto = new DepartmentDto2
        {
            DepartmentName = "Department name",
            DepartmentShortName = "Department short name",
            Id = _departmentId
        };

        _department = Department.Create(
            _departmentDto.DepartmentName,
            _departmentDto.DepartmentShortName,
            _departmentId,
            "Comments").Value;

        _departmentRepositoryMock = new Mock<IDepartmentRepository>();
        _departmentRepositoryMock.Setup(repository => repository.ExistsAsync(_departmentId, _cancellationToken))
            .ReturnsAsync(true);
        _departmentRepositoryMock.Setup(repository => repository.GetByIdAsync(_departmentId, _cancellationToken))
            .ReturnsAsync(_department);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditDepartment.CommandHandler(_departmentRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_DepartmentIdNotExist_ReturnsFailure()
    {
        // Arrange
        _departmentDto.Id = _nonExistentDepartmentId; 
        var command = new EditDepartment.Command(_departmentDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(DepartmentErrors.NotFound(_nonExistentDepartmentId).Code));
        }
    }

    [Test]
    public async Task Handle_DepartmentSuccessfullyEdited_ReturnsSuccessResult()
    {
        // Arrange
        var command = new EditDepartment.Command(_departmentDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}