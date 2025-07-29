using Application.Abstractions.Data;
using Application.UseCases.Grades;
using Application.UseCases.DTOs;
using Domain.Aggregates.Grades;
using Moq;

namespace Tests.Application.UseCases.Grades;

[TestFixture]
public class EditGradeTests
{
    private readonly GradeId _gradeId = new (Guid.CreateVersion7());
    private readonly GradeId _nonExistentGradeId = new (Guid.CreateVersion7());
    
    private GradeDto2 _gradeDto;
    private Grade _grade;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IGradeRepository> _gradeRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditGrade.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _gradeDto = new GradeDto2
        {
            GradeName = "Grade name",
            GradeShortName = "Grade short name",
            Id = _gradeId
        };

        _grade = Grade.Create(
            _gradeDto.GradeName,
            _gradeDto.GradeShortName,
            _gradeId,
            "Comments").Value;

        _gradeRepositoryMock = new Mock<IGradeRepository>();
        _gradeRepositoryMock.Setup(repository => repository.ExistsAsync(_gradeId, _cancellationToken))
            .ReturnsAsync(true);
        _gradeRepositoryMock.Setup(repository => repository.GetByIdAsync(_gradeId, _cancellationToken))
            .ReturnsAsync(_grade);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditGrade.CommandHandler(_gradeRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_GradeIdNotExist_ReturnsFailure()
    {
        // Arrange
        _gradeDto.Id = _nonExistentGradeId; 
        var command = new EditGrade.Command(_gradeDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(GradeErrors.NotFound(_nonExistentGradeId).Code));
        }
    }

    [Test]
    public async Task Handle_GradeSuccessfullyEdited_ReturnsSuccessResult()
    {
        // Arrange
        var command = new EditGrade.Command(_gradeDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}