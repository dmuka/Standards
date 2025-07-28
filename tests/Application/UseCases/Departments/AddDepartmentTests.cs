using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Departments;
using Core.Results;
using Domain.Aggregates.Departments;
using Moq;

namespace Tests.Application.UseCases.Departments;

[TestFixture]
public class AddDepartmentTests
{
    private const string DepartmentNameValue = "Department name";
    private const string DepartmentShortNameValue = "Department short name";
    
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());
    
    private DepartmentDto2 _departmentDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IDepartmentRepository> _departmentRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddDepartment.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _departmentDto = new DepartmentDto2
        {
            DepartmentName = DepartmentNameValue,
            DepartmentShortName = DepartmentShortNameValue,
            Id = _departmentId
        };
        
        _departmentRepositoryMock = new Mock<IDepartmentRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddDepartment.CommandHandler(_departmentRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_DepartmentSuccessfullyAdded_ReturnsSuccessResult()
    {
        // Arrange
        var command = new AddDepartment.Command(_departmentDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _departmentRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Department>(), _cancellationToken), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(_cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_DepartmentCreationFails_ReturnsZero()
    {
        // Arrange
        _departmentDto.DepartmentName = "";
        var command = new AddDepartment.Command(_departmentDto);

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