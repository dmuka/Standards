using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using FluentValidation;
using FluentValidation.TestHelper;
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

    private IRequestHandler<Delete.Command<VerificationJournalItem>, int> _handler;
    private IValidator<Delete.Command<VerificationJournalItem>> _validator;

    [SetUp]
    public void Setup()
    {
        _verificationJournalItem = VerificationJournalItems[0];

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetByIdAsync<VerificationJournalItem>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult<VerificationJournalItem?>(_verificationJournalItem));
        _repository.Setup(repository => repository.DeleteAsync(_verificationJournalItem, _cancellationToken));
        _repository.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Delete.CommandHandler<VerificationJournalItem>(_repository.Object, _cacheService.Object);
        _validator = new Delete.CommandValidator<VerificationJournalItem>(_repository.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var command = new Delete.Command<VerificationJournalItem>(IdInDb);

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
        
    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var command = new Delete.Command<VerificationJournalItem>(IdInDb);

        // Act
        _handler.Handle(command, _cancellationToken);

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
        var command = new Delete.Command<VerificationJournalItem>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
        
    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdNotInDB_ShouldHaveValidationError(int id)
    {
        // Arrange
        var command = new Delete.Command<VerificationJournalItem>(id);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(verificationJournalItem => verificationJournalItem.Id);
    }
}