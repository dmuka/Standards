using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Places;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Places;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int IdNotInDb = 12;
    
    private PlaceDto _place;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Create.Command, int> _handler;
    private IValidator<Create.Command> _validator;

    [SetUp]
    public void Setup()
    {
        _place = PlaceDtos[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(command => command.AddAsync(_place, _cancellationToken));
        _repositoryMock.Setup(command => command.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Create.CommandHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Create.CommandValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var command = new Create.Command(_place);
        var expected = 1;

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var commandObject = new Create.Command(_place);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(command => command.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(commandObject, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfAddressIsNull_ShouldHaveValidationError(string? address)
    {
        // Arrange
        _place.Address = address;

        var commandObject = new Create.Command(_place);

        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Place.Address);
    }

    [Test]
    public void Validator_IfAddressIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _place.Address = Cases.Length201;

        var commandObject = new Create.Command(_place);


        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Place.Address);
    }

    [Test]
    public void Validator_IfAddressIsShorterThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _place.Address = Cases.Length19;

        var commandObject = new Create.Command(_place);


        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Place.Address);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _place.Name = name;

        var commandObject = new Create.Command(_place);

        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Place.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _place.Name = Cases.Length201;

        var commandObject = new Create.Command(_place);


        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Place.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _place.ShortName = shortName;

        var commandObject = new Create.Command(_place);

        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Place.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _place.ShortName = Cases.Length101;

        var commandObject = new Create.Command(_place);

        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Place.ShortName);
    }

    [Test]
    public void Validator_IfContactIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _place.ContactIds = new List<int>();

        var commandObject = new Create.Command(_place);

        // Act
        var result = _validator.TestValidateAsync(commandObject, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(command => command.Place.ContactIds);
    }
}