using Application.Abstractions.Cache;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;
using Unit = Domain.Models.Unit;

namespace Tests.Application.CQRS.Units;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;
        
    private IList<Unit> _departments;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
        
    private IRequestHandler<GetById.Query<Unit>, Unit> _handler;
    private IValidator<GetById.Query<Unit>> _validator;

    [SetUp]
    public void Setup()
    {
        _departments = Units;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Unit>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_departments.First(_ => _.Id == IdInDb)));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetById<Unit>(Cache.Units, IdInDb)).Returns(Units[0]);

        _handler = new GetById.QueryHandler<Unit>(_repository.Object, _cacheService.Object);
        _validator = new GetById.QueryValidator<Unit>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Unit>(IdInDb);
        var expected = _departments.First(_ => _.Id == IdInDb);

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
        var query = new GetById.Query<Unit>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfUnitInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Unit>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Units[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Unit>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById.Query<Unit>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
}