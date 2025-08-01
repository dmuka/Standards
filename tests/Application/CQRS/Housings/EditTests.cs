﻿using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.DTOs;
using Application.UseCases.Housings;
using Domain.Constants;
using Domain.Models.Housings;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.Housings;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;

    private HousingDto _housing;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Edit.Query, int> _handler;
    private IValidator<Edit.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _housing = HousingDtos[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<HousingDto>(ValidId, _cancellationToken)).ReturnsAsync(_housing);
        _repositoryMock.Setup(_ => _.Update(_housing));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).ReturnsAsync(1);

        _cacheService = new Mock<ICacheService>();
            
        _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheService.Object);
        _validator = new Edit.QueryValidator(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Edit.Query(_housing);
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
        var query = new Edit.Query(_housing);

        // Act
        _handler.Handle(query, _cancellationToken);

        // Assert
        //_repositoryMock.Verify(repository => repository.GetQueryable<Department>(), Times.Once);
        _repositoryMock.Verify(repository => repository.GetQueryable<Room>(), Times.Once);
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<Housing>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Housings), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Edit.Query(_housing);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfHousingDtoIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _housing = null!;

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _housing.Id = default;

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _housing.Name = name!;

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _housing.Name = Cases.Length201;

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _housing.ShortName = shortName!;

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _housing.ShortName = Cases.Length101;

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto.ShortName);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfAddressIsNull_ShouldHaveValidationError(string? address)
    {
        // Arrange
        _housing.Address = address!;

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Address);
    }

    [Test]
    public void Validator_IfFloorsCountIsZero_ShouldHaveValidationError()
    {
        // Arrange
        _housing.FloorsCount = default;

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto.FloorsCount);
    }

    // [Test]
    // public void Validator_IfDepartmentIdsIsEmpty_ShouldHaveValidationError()
    // {
    //     // Arrange
    //     _housing.DepartmentIds = new List<int>();
    //
    //     var query = new Edit.Query(_housing);
    //
    //     // Act
    //     var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;
    //
    //     // Assert
    //     result.ShouldHaveValidationErrorFor(_ => _.HousingDto.DepartmentIds);
    // }

    [Test]
    public void Validator_IfRoomIdsIsEmpty_ShouldHaveValidationError()
    {
        // Arrange
        _housing.RoomIds = new List<int>();

        var query = new Edit.Query(_housing);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.HousingDto.RoomIds);
    }
}