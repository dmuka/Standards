﻿using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;
using Standards.CQRS.Tests.Common;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Sectors;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    
    private RoomDto _roomDto;
    private Room _room;
    private Housing _housing;
    private Sector _sector;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheMock;

    private IRequestHandler<Edit.Query, int> _handler;
    private IValidator<Edit.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _roomDto = RoomDtos[0];
        _roomDto.HousingId = IdInDb;
        _roomDto.Height = 1;
        _roomDto.Width = 1;
        _roomDto.Length = 1;
        _roomDto.Floor = 1;
        _roomDto.PersonIds = new List<int>() { 1, 2, 3 };
        _roomDto.WorkplaceIds = new List<int>() { 1, 2, 3, 4 };
        
        _room = Rooms[0];

        _housing = Housings[0];

        _sector = Sectors[0];

        _cancellationToken = new CancellationToken();

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<Room>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_room));
        _repositoryMock.Setup(_ => _.GetByIdAsync<Housing>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_housing));
        _repositoryMock.Setup(_ => _.GetByIdAsync<Sector>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_sector));
        _repositoryMock.Setup(_ => _.Update(_room));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheMock = new Mock<ICacheService>();

        _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheMock.Object);
        _validator = new Edit.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Edit.Query(_roomDto);
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
        var query = new Edit.Query(_roomDto);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Housing>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.GetQueryable<Person>(), Times.Once);
        _repositoryMock.Verify(repository => repository.GetQueryable<Workplace>(), Times.Once);
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<Room>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Edit.Query(_roomDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(default(int)));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(default(int)));
    }

    [Test]
    public void Validator_IfRoomDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _roomDto = null;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _roomDto.Id = id;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Id);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfHousingIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _roomDto.HousingId = id;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.HousingId);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfSectorIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _roomDto.SectorId = id;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.SectorId);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _roomDto.Name = name;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _roomDto.Name = Cases.Length201;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _roomDto.ShortName = shortName;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _roomDto.Name = Cases.Length101;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Name);
    }

    [Test]
    public void Validator_IfPersonIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _roomDto.PersonIds = new List<int>();

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.PersonIds);
    }

    [Test]
    public void Validator_IfWorkplaceIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _roomDto.WorkplaceIds = new List<int>();

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.WorkplaceIds);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfFloorIsInvalid_ShouldHaveValidationError(int floor)
    {
        // Arrange
        _roomDto.Floor = floor;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Floor);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfLengthIsInvalid_ShouldHaveValidationError(int length)
    {
        // Arrange
        _roomDto.Length = length;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Length);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfHeightIsInvalid_ShouldHaveValidationError(int height)
    {
        // Arrange
        _roomDto.Height = height;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Height);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfWidthIsInvalid_ShouldHaveValidationError(int width)
    {
        // Arrange
        _roomDto.Width = width;

        var query = new Edit.Query(_roomDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Width);
    }
}