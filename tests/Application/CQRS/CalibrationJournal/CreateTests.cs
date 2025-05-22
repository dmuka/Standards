using Application.Abstractions.Cache;
using Application.UseCases.CalibrationsJournal;
using Domain.Models.DTOs;
using Domain.Models.MetrologyControl;
using Domain.Models.Standards;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.CalibrationJournal;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
    
    private CalibrationJournalItem _calibrationJournalItem;
    private CalibrationJournalItemDto _calibrationJournalItemDto;
    
    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheMock;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _calibrationJournalItem = CalibrationJournalItems[0];
        
        _calibrationJournalItemDto = CalibrationJournalItemDtos[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.AddAsync(_calibrationJournalItem, _cancellationToken));
        _repositoryMock.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));
        _repositoryMock.Setup(repository => repository.GetByIdAsync<Standard>(ValidId, _cancellationToken)).ReturnsAsync(Standards[0]);
        _repositoryMock.Setup(repository => repository.GetByIdAsync<Place>(ValidId, _cancellationToken)).ReturnsAsync(Places[0]);

        _cacheMock = new Mock<ICacheService>();
        
        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheMock.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_calibrationJournalItemDto);
        var expected = 1;

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Create.Query(_calibrationJournalItemDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void CalibrationJournalItemDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _calibrationJournalItemDto = null;

        var query = new Create.Query(_calibrationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CalibrationJournalItemDto);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfCertificateIdIsNullOrEmpty_ShouldHaveValidationError(string? id)
    {
        // Arrange
        _calibrationJournalItemDto.CertificateId = id;

        var query = new Create.Query(_calibrationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CalibrationJournalItemDto.CertificateId);
    }

    [Test]
    public void Validator_IfCertificateIdIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _calibrationJournalItemDto.CertificateId = Cases.Length21;

        var query = new Create.Query(_calibrationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CalibrationJournalItemDto.CertificateId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfStandardIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _calibrationJournalItemDto.StandardId = id;

        var query = new Create.Query(_calibrationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CalibrationJournalItemDto.StandardId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfPlaceIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _calibrationJournalItemDto.PlaceId = id;

        var query = new Create.Query(_calibrationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CalibrationJournalItemDto.PlaceId);
    }

    [Test, TestCaseSource(nameof(MinOrInPast))]
    public void Validator_IfDateIsInvalidOrInPast_ShouldHaveValidationError(DateTime date)
    {
        // Arrange
        _calibrationJournalItemDto.Date = date;

        var query = new Create.Query(_calibrationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CalibrationJournalItemDto.Date);
    }

    [Test, TestCaseSource(nameof(MinOrInPast))]
    public void Validator_IfValidToIsInvalidOrLessThanDate_ShouldHaveValidationError(DateTime date)
    {
        // Arrange
        _calibrationJournalItemDto.ValidTo = date;

        var query = new Create.Query(_calibrationJournalItemDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.CalibrationJournalItemDto.ValidTo);
    }
}