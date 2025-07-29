using Application.UseCases.Grades;
using Domain.Aggregates.Grades;
using Moq;

namespace Tests.Application.UseCases.Grades;

[TestFixture]
public class GetGradeByIdTests
{
    private const string GradeNameValue = "Grade name";
    private const string GradeShortNameValue = "Grade short name";
    
    private readonly Guid _invalidGradeIdGuid = Guid.CreateVersion7();
    private GradeId _invalidGradeId;
    
    private readonly Guid _validGradeIdGuid = Guid.CreateVersion7();
    private GradeId _validGradeId;

    private Grade _grade;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IGradeRepository> _gradeRepositoryMock;
    
    private GetGradeById.QueryHandler _handler;

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
        
        _handler = new GetGradeById.QueryHandler(_gradeRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_GradeExists_ReturnsGrade()
    {
        // Arrange
        var query = new GetGradeById.Query(_validGradeId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_validGradeId));
            Assert.That(result.Value.Name.Value, Is.EqualTo(GradeNameValue));
            Assert.That(result.Value.ShortName.Value, Is.EqualTo(GradeShortNameValue));
        }
    }

    [Test]
    public async Task Handle_GradeDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetGradeById.Query(_invalidGradeId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(GradeErrors.NotFound(_invalidGradeId)));
        }
    }
}