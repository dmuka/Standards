using Application.UseCases.Grades;
using Domain.Aggregates.Grades;
using Moq;

namespace Tests.Application.UseCases.Grades;

[TestFixture]
public class GetAllGradesTests
{
    private readonly GradeId _gradeId1 = new (Guid.CreateVersion7());
    private readonly GradeId _gradeId2 = new (Guid.CreateVersion7());
 
    private Grade _grade1;
    private Grade _grade2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IGradeRepository> _gradeRepositoryMock;
    
    private GetAllGrades.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _grade1 = Grade.Create(
            "Grade name 1",
            "Grade short name 1",
            _gradeId1,
            "").Value;
        _grade2 = Grade.Create(
            "Grade name 2",
            "Grade short name 2",
            _gradeId2,
            "").Value;

        _gradeRepositoryMock = new Mock<IGradeRepository>();
        _gradeRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_grade1, _grade2]);
        
        _handler = new GetAllGrades.QueryHandler(_gradeRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllGrades()
    {
        // Arrange
        var query = new GetAllGrades.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Grade>(h => h.Id == _grade1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Grade>(h => h.Id == _grade2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoGradesExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllGrades.Query();
        _gradeRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
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