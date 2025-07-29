using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Grades;
using Core.Results;
using Domain.Aggregates.Grades;
using Moq;

namespace Tests.Application.UseCases.Grades;

[TestFixture]
public class AddGradeTests
{
    private const string GradeNameValue = "Grade name";
    private const string GradeShortNameValue = "Grade short name";
    
    private readonly GradeId _gradeId = new (Guid.CreateVersion7());
    
    private GradeDto2 _gradeDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IGradeRepository> _gradeRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddGrade.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _gradeDto = new GradeDto2
        {
            GradeName = GradeNameValue,
            GradeShortName = GradeShortNameValue,
            Id = _gradeId
        };
        
        _gradeRepositoryMock = new Mock<IGradeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddGrade.CommandHandler(_gradeRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_GradeSuccessfullyAdded_ReturnsSuccessResult()
    {
        // Arrange
        var command = new AddGrade.Command(_gradeDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _gradeRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Grade>(), _cancellationToken), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(_cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_GradeCreationFails_ReturnsZero()
    {
        // Arrange
        _gradeDto.GradeName = "";
        var command = new AddGrade.Command(_gradeDto);

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