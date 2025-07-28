using Application.UseCases.Persons;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Users;
using Moq;

namespace Tests.Application.UseCases.Persons;

[TestFixture]
public class GetAllPersonsTests
{
    private readonly PersonId _personId1 = new (Guid.CreateVersion7());
    private readonly PersonId _personId2 = new (Guid.CreateVersion7());
    private readonly UserId _userId1 = new (Guid.CreateVersion7());
    private readonly UserId _userId2 = new (Guid.CreateVersion7());
 
    private Person _person1;
    private Person _person2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IPersonRepository> _personRepositoryMock;
    
    private GetAllPersons.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _person1 = Person.Create(
            "Person name 1",
            null,
            "Person last name 1",
            null,
            _personId1,
            _userId1).Value;
        _person2 = Person.Create(
            "Person name 2",
            null,
            "Person last name 2",
            null,
            _personId2,
            _userId2).Value;

        _personRepositoryMock = new Mock<IPersonRepository>();
        _personRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_person1, _person2]);
        
        _handler = new GetAllPersons.QueryHandler(_personRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllPersons()
    {
        // Arrange
        var query = new GetAllPersons.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Person>(h => h.Id == _person1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Person>(h => h.Id == _person2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoPersonsExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllPersons.Query();
        _personRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
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