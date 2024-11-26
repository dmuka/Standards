using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.Persons;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Persons;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;
        
    private IList<Person> _person;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
        
    private IRequestHandler<GetById.Query<Person>, Person> _handler;
    private IValidator<GetById.Query<Person>> _validator;

    [SetUp]
    public void Setup()
    {
        _person = Persons;

        _cancellationToken = new CancellationToken();

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Person>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_person.First(_ => _.Id == IdInDb)));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetById<Person>(Cache.Persons, IdInDb)).Returns(Persons[0]);

        _handler = new GetById.QueryHandler<Person>(_repository.Object, _cacheService.Object);
        _validator = new GetById.QueryValidator<Person>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Person>(IdInDb);
        var expected = _person.First(_ => _.Id == IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ReturnResult(int id)
    {
        // Arrange
        var query = new GetById.Query<Person>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfPersonInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Person>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Persons[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Person>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById.Query<Person>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
}