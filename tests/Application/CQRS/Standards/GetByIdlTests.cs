using Application.Abstractions.Cache;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Services;
using Domain.Models.Standards;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Standards;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;

    private Mock<IRepository> _repository;
    private Mock<ICacheService> _cacheMock;
    private Mock<ILogger<GetById>> _logger;
    
    private CancellationToken _cancellationToken;
    
    private List<Standard?> _standards;
    
    private IRequestHandler<GetById.Query<Standard>, Standard> _handler;
    private IValidator<GetById.Query<Standard>> _validator;

    [SetUp]
    public void Setup()
    {
        _standards = Standards;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetByIdAsync<Standard>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_standards.First(_ => _.Id == IdInDb)));

        _cacheMock = new Mock<ICacheService>();
        _cacheMock.Setup(cache => cache.GetById<Standard>(Cache.Standards, IdInDb)).Returns(Standards[0]);

        _logger = new Mock<ILogger<GetById>>();

        _handler = new GetById.QueryHandler<Standard>(_repository.Object, _cacheMock.Object, _logger.Object); 
        _validator = new GetById.QueryValidator<Standard>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Standard>(IdInDb);
        var expected = _standards.First(_ => _.Id == IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ReturnResult(int id)
    {
        // Arrange
        var query = new GetById.Query<Standard>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfStandardsInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Standard>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Standards[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Standard>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ShoulThrowException()
    {
        // Arrange
        var query = new GetById.Query<Standard>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.Null);
    }
}