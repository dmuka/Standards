using Application.Abstractions.Cache;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.CalibrationJournal;

public class DeleteTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    private VerificationJournalItem _verificationJournalItem;
        
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Delete.Query<VerificationJournalItem>, int> _handler;
    private IValidator<Delete.Query<VerificationJournalItem>> _validator;

    [SetUp]
    public void Setup()
    {
        _verificationJournalItem = VerificationJournalItems[0];

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<VerificationJournalItem>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_verificationJournalItem));
        _repository.Setup(_ => _.DeleteAsync(_verificationJournalItem, _cancellationToken));
        _repository.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Delete.QueryHandler<VerificationJournalItem>(_repository.Object, _cacheService.Object);
        _validator = new Delete.QueryValidator<VerificationJournalItem>(_repository.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Delete.Query<VerificationJournalItem>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
        
    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var query = new Delete.Query<VerificationJournalItem>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        _repository.Verify(repository => repository.GetByIdAsync<VerificationJournalItem>(IdInDb, _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<VerificationJournalItem>(), _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.VerificationJournal), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Delete.Query<VerificationJournalItem>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
        
    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdNotInDB_ShouldHaveValidationError(int id)
    {
        // Arrange
        var query = new Delete.Query<VerificationJournalItem>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }
}