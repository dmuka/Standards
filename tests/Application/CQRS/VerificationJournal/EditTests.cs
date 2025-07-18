using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.VerificationsJournal;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using Domain.Models.Standards;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.VerificationJournal;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    
    private VerificationJournalItemDto _verificationJournalItemDto;
    private VerificationJournalItem _verificationJournalItem;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<ILogger<Edit>> _logger;

    private IRequestHandler<Edit.Command, int> _handler;
    private IValidator<Edit.Command> _validator;

    [SetUp]
    public void Setup()
    {
        _verificationJournalItemDto = VerificationJournalItemDtos[0];
        _verificationJournalItem = VerificationJournalItems[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<VerificationJournalItem>(IdInDb, _cancellationToken)).ReturnsAsync(_verificationJournalItem);
        _repositoryMock.Setup(_ => _.GetByIdAsync<Standard>(IdInDb, _cancellationToken)).ReturnsAsync(Standards[0]);
        _repositoryMock.Setup(_ => _.GetByIdAsync<Place>(IdInDb, _cancellationToken)).ReturnsAsync(Places[0]);
        _repositoryMock.Setup(_ => _.Update(_verificationJournalItem));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();
        
        _logger = new Mock<ILogger<Edit>>();

        _handler = new Edit.CommandHandler(_repositoryMock.Object, _cacheService.Object, _logger.Object);
        _validator = new Edit.CommandValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Edit.Command(_verificationJournalItemDto);
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
        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Standard>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Place>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<VerificationJournalItem>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.VerificationJournal), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Edit.Command(_verificationJournalItemDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void VerificationJournalItemDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _verificationJournalItemDto = null!;

        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VerificationJournalItemDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _verificationJournalItemDto.Id = id;

        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VerificationJournalItemDto.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfCertificateIdIsNullOrEmpty_ShouldHaveValidationError(string? id)
    {
        // Arrange
        _verificationJournalItemDto.CertificateId = id!;

        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VerificationJournalItemDto.CertificateId);
    }

    [Test]
    public void Validator_IfCertificateIdIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _verificationJournalItemDto.CertificateId = Cases.Length21;

        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VerificationJournalItemDto.CertificateId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfStandardIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _verificationJournalItemDto.StandardId = id;

        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VerificationJournalItemDto.StandardId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfPlaceIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _verificationJournalItemDto.PlaceId = id;

        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VerificationJournalItemDto.PlaceId);
    }

    [Test, TestCaseSource(nameof(MinOrInPast))]
    public void Validator_IfDateIsInvalidOrInPast_ShouldHaveValidationError(DateTime date)
    {
        // Arrange
        _verificationJournalItemDto.Date = date;

        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VerificationJournalItemDto.Date);
    }

    [Test, TestCaseSource(nameof(MinOrInPast))]
    public void Validator_IfValidToIsInvalidOrLessThanDate_ShouldHaveValidationError(DateTime date)
    {
        // Arrange
        _verificationJournalItemDto.ValidTo = date;

        var query = new Edit.Command(_verificationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.VerificationJournalItemDto.ValidTo);
    }
}