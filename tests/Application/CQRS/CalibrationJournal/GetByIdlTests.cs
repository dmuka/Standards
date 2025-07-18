using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using Domain.Models.Services;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.CalibrationJournal;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;

    private Mock<IRepository> _repository;
    private Mock<ICacheService> _cacheMock;
    private Mock<ILogger<GetById>> _logger;
    
    private CancellationToken _cancellationToken;
    
    private List<VerificationJournalItem> _services;
    
    private IRequestHandler<GetById.Query<VerificationJournalItem>, VerificationJournalItem> _handler;
    private IValidator<GetById.Query<VerificationJournalItem>> _validator;

    [SetUp]
    public void Setup()
    {
        _services = VerificationJournalItems;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<VerificationJournalItem>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_services.First(_ => _.Id == IdInDb)));

        _cacheMock = new Mock<ICacheService>();
        _cacheMock.Setup(cache => cache.GetById<VerificationJournalItem>(Cache.ServiceJournal, IdInDb)).Returns(VerificationJournalItems[0]);

        _logger = new Mock<ILogger<GetById>>();

        _handler = new GetById.QueryHandler<VerificationJournalItem>(_repository.Object, _cacheMock.Object, _logger.Object); 
        _validator = new GetById.QueryValidator<VerificationJournalItem>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<VerificationJournalItem>(IdInDb);
        var expected = _services.First(_ => _.Id == IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdIsInvalid_ReturnResult(int id)
    {
        // Arrange
        var query = new GetById.Query<VerificationJournalItem>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfServiceJousnalItemInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<VerificationJournalItem>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(VerificationJournalItems[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Service>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ShoulThrowException()
    {
        // Arrange
        var query = new GetById.Query<VerificationJournalItem>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.Null);
    }
}