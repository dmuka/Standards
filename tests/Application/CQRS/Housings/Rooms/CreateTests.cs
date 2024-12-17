using Application.Abstractions.Cache;
using Application.CQRS.Rooms;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Housings;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Housings.Rooms;

[TestFixture]
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
    
    private Room _room;
    private RoomDto _roomDto;
    private Housing _housing;
    private Sector _sector;
    
    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheMock;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _room = Rooms[0];
        
        _roomDto = RoomDtos[0];

        _housing = Housings[0];

        _sector = Sectors[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.AddAsync(_room, _cancellationToken));
        _repositoryMock.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));
        _repositoryMock.Setup(repository => repository.GetByIdAsync<Housing>(ValidId, _cancellationToken)).ReturnsAsync(_housing);
        _repositoryMock.Setup(repository => repository.GetByIdAsync<Sector>(ValidId, _cancellationToken)).ReturnsAsync(_sector);

        _cacheMock = new Mock<ICacheService>();
        
        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheMock.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_roomDto);
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
        var query = new Create.Query(_roomDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _roomDto.Name = name;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;
        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _roomDto.Name = Cases.Length201;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _roomDto.ShortName = shortName;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _roomDto.ShortName = Cases.Length101;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.ShortName);
    }

    [Test]
    public void Validator_IfRoomDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _roomDto = null;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfWidthIsInvalid_ShouldHaveValidationError(int width)
    {
        // Arrange
        _roomDto.Width = width;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.Width);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfLengthIsInvalid_ShouldHaveValidationError(int length)
    {
        // Arrange
        _roomDto.Length = length;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.Length);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfHeightIsInvalid_ShouldHaveValidationError(int height)
    {
        // Arrange
        _roomDto.Height = height;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.Height);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfHousingIdIsInvalid_ShouldHaveValidationError(int housingId)
    {
        // Arrange
        _roomDto.HousingId = housingId;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.HousingId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfSectorIdIsInvalid_ShouldHaveValidationError(int sectorId)
    {
        // Arrange
        _roomDto.SectorId = sectorId;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.SectorId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfFloorIsInvalid_ShouldHaveValidationError(int floor)
    {
        // Arrange
        _roomDto.Floor = floor;

        var query = new Create.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Room.Floor);
    }
}