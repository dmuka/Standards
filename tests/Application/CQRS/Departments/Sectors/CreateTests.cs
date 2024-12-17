using Application.Abstractions.Cache;
using Application.CQRS.Sectors;
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
public class CreateTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;
    
    private Sector _sector;
    private SectorDto _sectorDto;
    private Department _department;
    //private Sector _sector;
    
    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheMock;

    private IRequestHandler<Create.Query, int> _handler;
    private IValidator<Create.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _sector = Sectors[0];
        
        _sectorDto = SectorDtos[0];

        _department = Departments[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(repository => repository.AddAsync(_sector, _cancellationToken));
        _repositoryMock.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));
        _repositoryMock.Setup(repository => repository.GetByIdAsync<Department>(ValidId, _cancellationToken)).ReturnsAsync(_department);
        _repositoryMock.Setup(repository => repository.GetQueryable<Room>()).Returns(new List<Room> { Rooms[0], Rooms[1] }.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Person>()).Returns(new List<Person> { Persons[0], Persons[1], Persons[2] }.AsQueryable());
        _repositoryMock.Setup(repository => repository.GetQueryable<Workplace>()).Returns(new List<Workplace> { Workplaces[4], Workplaces[5] }.AsQueryable());

        _cacheMock = new Mock<ICacheService>();
        
        _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheMock.Object);
        _validator = new Create.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Create.Query(_sectorDto);
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
        var query = new Create.Query(_sectorDto);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfSectorDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto = null;

        var query = new Create.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _sectorDto.Name = name;

        var query = new Create.Query(_sectorDto);

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

        var query = new Create.Query(_sectorDto);

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

        var query = new Create.Query(_sectorDto);

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

        var query = new Create.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.ShortName);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfDepartmentIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _sectorDto.DepartmentId = id;

        var query = new Create.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.DepartmentId);
    }

    [Test]
    public void Validator_IfWorkplaceIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.WorkplaceIds = new List<int>();

        var query = new Create.Query(_sectorDto);

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

        var query = new Create.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.WorkplaceIds);
    }

    [Test]
    public void Validator_IfPersonIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.PersonIds = new List<int>();

        var query = new Create.Query(_sectorDto);

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

        var query = new Create.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.PersonIds);
    }

    [Test]
    public void Validator_IfRoomIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _sectorDto.RoomIds = new List<int>();

        var query = new Create.Query(_sectorDto);

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

        var query = new Create.Query(_sectorDto);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.SectorDto.RoomIds);
    }
}