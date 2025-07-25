﻿using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.Common.GenericCRUD;
using Core.Results;
using Domain.Constants;
using Domain.Models.Persons;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;
using Tests.Common.Constants;

namespace Tests.Application.CQRS.BaseEntities.Positions;

[TestFixture]
public class EditTests : BaseTestFixture
{
    private const int ValidId = 1;
    private const int IdNotInDb = 2;

    private Position _position;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<EditBaseEntity.Query<Position>, Result<int>> _handler;
    private IValidator<EditBaseEntity.Query<Position>> _validator;

    [SetUp]
    public void Setup()
    {
        _position = Positions[0];

        _cancellationToken = CancellationToken.None;

        _repositoryMock = new Mock<IRepository>();
        _repositoryMock.Setup(_ => _.GetByIdAsync<Position>(ValidId, _cancellationToken)).ReturnsAsync(_position);
        _repositoryMock.Setup(_ => _.Update(_position));
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).ReturnsAsync(1);

        _cacheService = new Mock<ICacheService>();
            
        _handler = new EditBaseEntity.QueryHandler<Position>(_repositoryMock.Object, _cacheService.Object);
        _validator = new EditBaseEntity.QueryValidator<Position>(_repositoryMock.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new EditBaseEntity.Query<Position>(_position);
        var expected = 1;

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Value, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var query = new EditBaseEntity.Query<Position>(_position);

        // Act
        _handler.Handle(query, _cancellationToken);

        // Assert
        _repositoryMock.Verify(repository => repository.Update(It.IsAny<Position>()), Times.Once);
        _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Positions), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnZero()
    {
        // Arrange
        var query = new EditBaseEntity.Query<Position>(_position);
        _cancellationToken = new CancellationToken(true);
        _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(0));

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Value, Is.EqualTo(0));
    }

    [Test]
    public void Validator_IfPositionIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _position = null!;

        var query = new EditBaseEntity.Query<Position>(_position);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        _position.Id = default;

        var query = new EditBaseEntity.Query<Position>(_position);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.Id);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfNameIsNullOrEmpty_ShouldHaveValidationError(string? name)
    {
        // Arrange
        _position.Name = name!;

        var query = new EditBaseEntity.Query<Position>(_position);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.Name);
    }

    [Test]
    public void Validator_IfNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _position.Name = Cases.Length201;

        var query = new EditBaseEntity.Query<Position>(_position);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.Name);
    }

    [Test, TestCaseSource(nameof(NullOrEmptyString))]
    public void Validator_IfShortNameIsNullOrEmpty_ShouldHaveValidationError(string? shortName)
    {
        // Arrange
        _position.ShortName = shortName!;

        var query = new EditBaseEntity.Query<Position>(_position);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.ShortName);
    }

    [Test]
    public void Validator_IfShortNameIsLongerThanRequired_ShouldHaveValidationError()
    {
        // Arrange
        _position.ShortName = Cases.Length101;

        var query = new EditBaseEntity.Query<Position>(_position);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Entity.ShortName);
    }
}