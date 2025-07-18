using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Places;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Places;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;

    private PlaceDto _placeDto;
    private Place _place;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Edit.Command, int> _handler;
    private IValidator<Edit.Command> _validator;

    [SetUp]
    public void Setup()
    {
        _placeDto = PlaceDtos[0];
        _place = Places[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.GetByIdAsync<Place>(ValidId, _cancellationToken)).Returns(Task.FromResult<Place?>(_place));
        _repositoryMock.Setup(repository => repository.Update(_placeDto));
        _repositoryMock.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();
            
        _handler = new Edit.CommandHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Edit.CommandValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var command = new Edit.Command(_placeDto);
        var expected = 1;

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var command = new Edit.Command(_placeDto);

        // Act
        _handler.Handle(command, _cancellationToken);

        // Assert
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<Place>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Places), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnZero()
    {
        // Arrange
        var command = new Edit.Command(_placeDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfPlaceIsNull_ShouldHaveValidationError()
    {
        // Arrange
        var command = new Edit.Command(null);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _placeDto.Id = 0;

        var command = new Edit.Command(_placeDto);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfAddressIsNull_ShouldHaveValidationError(string? address)
    {
        // Arrange
        _place.Address = address;

        var commandObject = new Edit.Command(_placeDto);

        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.Address);
    }

    [Test]
    public void Validator_IfAddressIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _placeDto.Address = Cases.Length201;

        var command = new Edit.Command(_placeDto);


        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.Address);
    }

    [Test]
    public void Validator_IfAddressIsShorterThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _placeDto.Address = Cases.Length19;

        var command = new Edit.Command(_placeDto);


        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.Address);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _placeDto.Name = name;

        var command = new Edit.Command(_placeDto);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _placeDto.Name = Cases.Length201;

        var command = new Edit.Command(_placeDto);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _placeDto.ShortName = shortName;

        var command = new Edit.Command(_placeDto);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _placeDto.ShortName = Cases.Length101;

        var command = new Edit.Command(_placeDto);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.ShortName);
    }

    [Test]
    public void Validator_IfContactIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _placeDto.ContactIds = new List<int>();

        var command = new Edit.Command(_placeDto);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.PlaceDto.ContactIds);
    }
}