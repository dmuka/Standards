using Application.Abstractions.Data;
using Application.UseCases.Grades;
using Domain.Aggregates.Grades;
using Moq;

namespace Tests.Application.UseCases.Grades;

[TestFixture]
public class DeleteGradeTests
{
    private const string GradeNameValue = "Grade name";
    private const string GradeShortNameValue = "Grade short name";
    
    private readonly Guid _invalidGradeIdGuid = Guid.CreateVersion7();
    private GradeId _invalidGradeId;
    
    private readonly Guid _validGradeIdGuid = Guid.CreateVersion7();
    private GradeId _validGradeId;
    private readonly GradeId _gradeId = new (Guid.CreateVersion7());

    private Grade _grade;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private Mock<IGradeRepository> _gradeRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private DeleteGrade.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validGradeId = new GradeId(_validGradeIdGuid);
        _invalidGradeId = new GradeId(_invalidGradeIdGuid);
        
        _grade = Grade.Create(
                               GradeNameValue, 
                               GradeShortNameValue,
                               _validGradeId,
                               "Comments").Value;
        
        _gradeRepositoryMock = new Mock<IGradeRepository>();
        _gradeRepositoryMock.Setup(repository => repository.ExistsAsync(_validGradeId, _cancellationToken))
            .ReturnsAsync(true);
        _gradeRepositoryMock.Setup(repository => repository.GetByIdAsync(_validGradeId, _cancellationToken))
            .ReturnsAsync(_grade);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new DeleteGrade.CommandHandler(_gradeRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_GradeExists_DeletesGradeAndReturnsSuccess()
    {
        // Arrange
        var command = new DeleteGrade.Command(_validGradeId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _gradeRepositoryMock.Verify(repository => repository.ExistsAsync(_validGradeId, _cancellationToken), Times.Once);
            _gradeRepositoryMock.Verify(repository => repository.GetByIdAsync(_validGradeId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_GradeDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteGrade.Command(_invalidGradeId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(GradeErrors.NotFound(_invalidGradeId)));
        }
    }
}