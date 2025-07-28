using Application.Abstractions.Data;
using Application.UseCases.Persons;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Persons;
using Moq;

namespace Tests.Application.UseCases.Persons;

[TestFixture]
public class DeletePersonTests
{
    private const string PersonNameValue = "Person name";
    private const string PersonLastNameValue = "Person last name";
    
    private readonly Guid _invalidPersonIdGuid = Guid.CreateVersion7();
    private PersonId _invalidPersonId;
    
    private readonly Guid _validPersonIdGuid = Guid.CreateVersion7();
    private PersonId _validPersonId;
    private readonly DepartmentId _departmentId = new (Guid.CreateVersion7());

    private Person _person;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private DeletePerson.CommandHandler _handler;

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
                               _validPersonId,
                               _departmentId,
                               "Comments").Value;
        
        _personRepositoryMock = new Mock<IPersonRepository>();
        _personRepositoryMock.Setup(repository => repository.ExistsAsync(_validPersonId, _cancellationToken))
            .ReturnsAsync(true);
        _personRepositoryMock.Setup(repository => repository.GetByIdAsync(_validPersonId, _cancellationToken))
            .ReturnsAsync(_person);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new DeletePerson.CommandHandler(_personRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_PersonExists_DeletesPersonAndReturnsSuccess()
    {
        // Arrange
        var command = new DeletePerson.Command(_validPersonId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _personRepositoryMock.Verify(repository => repository.ExistsAsync(_validPersonId, _cancellationToken), Times.Once);
            _personRepositoryMock.Verify(repository => repository.GetByIdAsync(_validPersonId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_PersonDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeletePerson.Command(_invalidPersonId);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(PersonErrors.NotFound(_invalidPersonId)));
        }
    }
}