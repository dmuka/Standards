using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Persons;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Users;
using Moq;

namespace Tests.Application.UseCases.Persons;

[TestFixture]
public class EditPersonTests
{
    private readonly PersonId _personId = new (Guid.CreateVersion7());
    private readonly PersonId _nonExistentPersonId = new (Guid.CreateVersion7());
    private readonly UserId _userId = new (Guid.CreateVersion7());
    
    private PersonDto2 _personDto;
    private Person _person;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditPerson.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _personDto = new PersonDto2
        {
            FirstName = "Person first name",
            MiddleName = null,
            LastName = "Person last name",
            BirthdayDate = null,
            Id = _personId,
            UserId = _userId
        };

        _person = Person.Create(
            _personDto.FirstName,
            _personDto.MiddleName,
            _personDto.LastName,
            _personDto.BirthdayDate,
            _personDto.UserId,
            _personId,
            "Comments").Value;

        _personRepositoryMock = new Mock<IPersonRepository>();
        _personRepositoryMock.Setup(repository => repository.ExistsAsync(_personId, _cancellationToken))
            .ReturnsAsync(true);
        _personRepositoryMock.Setup(repository => repository.GetByIdAsync(_personId, _cancellationToken))
            .ReturnsAsync(_person);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditPerson.CommandHandler(_personRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_PersonIdNotExist_ReturnsFailure()
    {
        // Arrange
        _personDto.Id = _nonExistentPersonId; 
        var command = new EditPerson.Command(_personDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(PersonErrors.NotFound(_nonExistentPersonId).Code));
        }
    }

    [Test]
    public async Task Handle_PersonSuccessfullyEdited_ReturnsSuccessResult()
    {
        // Arrange
        var command = new EditPerson.Command(_personDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}