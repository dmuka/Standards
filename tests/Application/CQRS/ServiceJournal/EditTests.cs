using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.ServicesJournal;
using Domain.Constants;
using Domain.Models.Persons;
using Domain.Models.Services;
using Domain.Models.Standards;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.ServiceJournal;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    
    private ServiceJournalItemDto _serviceJournalItemDto;
    private ServiceJournalItem _serviceJournalItem;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Edit.Query, int> _handler;
    private IValidator<Edit.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _serviceJournalItemDto = ServiceJournalItemDtos[0];
        _serviceJournalItem = ServiceJournalItems[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<ServiceJournalItem>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_serviceJournalItem));
        _repositoryMock.Setup(_ => _.GetByIdAsync<Service>(IdInDb, _cancellationToken)).Returns(Task.FromResult(Services[0]));
        _repositoryMock.Setup(_ => _.GetByIdAsync<Standard>(IdInDb, _cancellationToken)).Returns(Task.FromResult(Standards[0]));
        _repositoryMock.Setup(_ => _.GetByIdAsync<Person>(IdInDb, _cancellationToken)).Returns(Task.FromResult(Persons[0]));
        _repositoryMock.Setup(_ => _.Update(_serviceJournalItem));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Edit.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Edit.Query(_serviceJournalItemDto);
        var expected = 1;

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Service>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Standard>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Person>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<ServiceJournalItem>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.ServiceJournal), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Edit.Query(_serviceJournalItemDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void ServiceJournalItemDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _serviceJournalItemDto = null;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _serviceJournalItemDto.Id = id;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _serviceJournalItemDto.Name = name;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _serviceJournalItemDto.Name = Cases.Length201;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _serviceJournalItemDto.ShortName = shortName;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _serviceJournalItemDto.ShortName = Cases.Length101;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.ShortName);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfServiceIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _serviceJournalItemDto.ServiceId = id;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.ServiceId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfStandardIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _serviceJournalItemDto.StandardId = id;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.StandardId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfPersonIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _serviceJournalItemDto.PersonId = id;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.PersonId);
    }

    [Test, TestCaseSource(nameof(MinOrInPast))]
    public void Validator_IfDateIsInvalidOrInPast_ShouldHaveValidationError(DateTime date)
    {
        // Arrange
        _serviceJournalItemDto.Date = date;

        var query = new Edit.Query(_serviceJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.ServiceJournalItemDto.Date);
    }
}