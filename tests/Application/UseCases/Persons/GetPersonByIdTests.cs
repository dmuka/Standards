using Application.UseCases.Persons;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Users;
using Moq;

namespace Tests.Application.UseCases.Persons;

[TestFixture]
public class GetPersonByIdTests
{
    private const string PersonNameValue = "Person name";
    private const string PersonLastNameValue = "Person last name";
    
    private readonly Guid _invalidPersonIdGuid = Guid.CreateVersion7();
    private PersonId _invalidPersonId;
    
    private readonly Guid _validPersonIdGuid = Guid.CreateVersion7();
    private PersonId _validPersonId;
    
    private readonly UserId _userId = new (Guid.CreateVersion7());

    private Person _person;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IPersonRepository> _sectorRepositoryMock;
    
    private GetPersonById.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validPersonId = new PersonId(_validPersonIdGuid);
        _invalidPersonId = new PersonId(_invalidPersonIdGuid);
        
        _person = Person.Create(
            PersonNameValue,
            null,
            PersonLastNameValue,
            null,
            _userId,
            _validPersonId,
            "Comments").Value;        

        _sectorRepositoryMock = new Mock<IPersonRepository>();
        _sectorRepositoryMock.Setup(repository => repository.ExistsAsync(_validPersonId, _cancellationToken))
            .ReturnsAsync(true);
        _sectorRepositoryMock.Setup(repository => repository.GetByIdAsync(_validPersonId, _cancellationToken))
            .ReturnsAsync(_person);
        
        _handler = new GetPersonById.QueryHandler(_sectorRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_PersonExists_ReturnsPerson()
    {
        // Arrange
        var query = new GetPersonById.Query(_validPersonId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_validPersonId));
            Assert.That(result.Value.FirstName.Value, Is.EqualTo(PersonNameValue));
            Assert.That(result.Value.LastName.Value, Is.EqualTo(PersonLastNameValue));
        }
    }

    [Test]
    public async Task Handle_PersonDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetPersonById.Query(_invalidPersonId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(PersonErrors.NotFound(_invalidPersonId)));
        }
    }
}