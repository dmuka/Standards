using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Quantities;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;
        
    private IList<Quantity> _departments;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<ILogger<GetById>> _logger;
        
    private IRequestHandler<GetById.Query<Quantity>, Quantity?> _handler;
    private IValidator<GetById.Query<Quantity>> _validator;

    [SetUp]
    public void Setup()
    {
        _departments = Quantities;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Quantity>(IdInDb, _cancellationToken))
            .ReturnsAsync(_departments.First(_ => _.Id == IdInDb));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetById<Quantity>(Cache.Quantities, IdInDb)).Returns(Quantities[0]);
        
        _logger = new Mock<ILogger<GetById>>();

        _handler = new GetById.QueryHandler<Quantity>(_repository.Object, _cacheService.Object, _logger.Object);
        _validator = new GetById.QueryValidator<Quantity>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Quantity>(IdInDb);
        var expected = _departments.First(_ => _.Id == IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdIsInvalid_ReturnResult(int id)
    {
        // Arrange
        var query = new GetById.Query<Quantity>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfQuantityInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Quantity>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Quantities[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Quantity>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ShoulThrowException()
    {
        // Arrange
        var query = new GetById.Query<Quantity>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.Null);
    }
}