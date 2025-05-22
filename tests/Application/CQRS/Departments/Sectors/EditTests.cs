using Application.Abstractions.Cache;
using Application.UseCases.Sectors;
using Domain.Constants;
using Domain.Models.Departments;
using Domain.Models.DTOs;
using Domain.Models.Housings;
using Domain.Models.Persons;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Departments.Sectors;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    
    private SectorDto _sectorDto;
    private Department _department;
    private Sector _sector;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Edit.Query, int> _handler;
    private IValidator<Edit.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _sectorDto = SectorDtos[0];
        _sector = Sectors[0];

        _department = Departments[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<Sector>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_sector));
        _repositoryMock.Setup(_ => _.GetByIdAsync<Department>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_department));
        _repositoryMock.Setup(repository => repository.GetQueryable<Room>()).Returns(new List<Room> { Rooms[0], Rooms[1] }.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Person>()).Returns(new List<Person> { Persons[0], Persons[1], Persons[2] }.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Workplace>()).Returns(new List<Workplace> { Workplaces[4], Workplaces[5] }.AsQueryable());
        _repositoryMock.Setup(_ => _.Update(_sector));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Edit.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Edit.Query(_sectorDto);
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
        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        _repositoryMock.Verify(repository => repository.GetByIdAsync<Department>(IdInDb, _cancellationToken), Times.Once);
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<Sector>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Sectors), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Edit.Query(_sectorDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void SectorDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto = null;

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _sectorDto.Id = id;

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.Id);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfDepartmentIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _sectorDto.DepartmentId = id;

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.DepartmentId);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _sectorDto.Name = name;

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.Name = Cases.Length201;

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _sectorDto.ShortName = shortName;

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.ShortName = Cases.Length101;

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.ShortName);
    }

    [Test]
    public void Validator_IfPersonIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.PersonIds = new List<int>();

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.PersonIds);
    }

    [Test]
    public void Validator_IfPersonIdIsNotInDb_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.PersonIds = new List<int> { IdNotInDb };

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.PersonIds);
    }

    [Test]
    public void Validator_IfWorkplaceIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.WorkplaceIds = new List<int>();

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.WorkplaceIds);
    }

    [Test]
    public void Validator_IfWorkplaceIdIsNotInDb_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.WorkplaceIds = new List<int> { IdNotInDb };

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.WorkplaceIds);
    }

    [Test]
    public void Validator_IfRoomIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.RoomIds = new List<int>();

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.RoomIds);
    }

    [Test]
    public void Validator_IfRoomIdIsNotInDb_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.RoomIds = new List<int> { IdNotInDb };

        var query = new Edit.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.RoomIds);
    }
}