using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Services;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.ServiceJournal;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;

    private Mock<IRepository> _repository;
    private Mock<ICacheService> _cacheMock;
    private Mock<ILogger<GetById>> _logger;
    
    private CancellationToken _cancellationToken;
    
    private List<ServiceJournalItem> _services;
    
    private IRequestHandler<GetById.Query<ServiceJournalItem>, ServiceJournalItem?> _handler;
    private IValidator<GetById.Query<ServiceJournalItem>> _validator;

    [SetUp]
    public void Setup()
    {
        _services = ServiceJournalItems;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<ServiceJournalItem>(IdInDb, _cancellationToken))
            .ReturnsAsync(_services.First(_ => _.Id == IdInDb));

        _cacheMock = new Mock<ICacheService>();
        _cacheMock.Setup(cache => cache.GetById<ServiceJournalItem>(Cache.ServiceJournal, IdInDb)).Returns(ServiceJournalItems[0]);

        _logger = new Mock<ILogger<GetById>>();

        _handler = new GetById.QueryHandler<ServiceJournalItem>(_repository.Object, _cacheMock.Object, _logger.Object); 
        _validator = new GetById.QueryValidator<ServiceJournalItem>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<ServiceJournalItem>(IdInDb);
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
        var query = new GetById.Query<ServiceJournalItem>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfServiceJousnalItemInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<ServiceJournalItem>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(ServiceJournalItems[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Service>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ShoulThrowException()
    {
        // Arrange
        var query = new GetById.Query<ServiceJournalItem>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.Null);
    }
}