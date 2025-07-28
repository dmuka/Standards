using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Persons;
using Core.Results;
using Domain.Aggregates.Departments;
using Domain.Aggregates.Persons;
using Domain.Aggregates.Users;
using Moq;

namespace Tests.Application.UseCases.Persons;

[TestFixture]
public class AddPersonTests
{
    private const string PersonNameValue = "Person name";
    private const string PersonLastNameValue = "Person last name";
    
    private readonly PersonId _personId = new (Guid.CreateVersion7());
    private readonly UserId _userId = new (Guid.CreateVersion7());
    
    private PersonDto2 _personDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<IPersonRepository> _personRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddPerson.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _personDto = new PersonDto2
        {
            FirstName = PersonNameValue,
            MiddleName = null,
            LastName = PersonLastNameValue,
            BirthdayDate = null,
            Id = _personId, 
            UserId = _userId
        };
        
        _personRepositoryMock = new Mock<IPersonRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddPerson.CommandHandler(_personRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_PersonSuccessfullyAdded_ReturnsSuccessResult()
    {
        // Arrange
        var command = new AddPerson.Command(_personDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _personRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Person>(), _cancellationToken), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(_cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_PersonCreationFails_ReturnsZero()
    {
        // Arrange
        _personDto.FirstName = "";
        var command = new AddPerson.Command(_personDto);

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