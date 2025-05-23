using Application.Abstractions.Cache;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Housings;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Housings;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;
        
    private IList<Housing> _housings;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
    private Mock<ILogger<GetById>> _logger;
        
    private IRequestHandler<GetById.Query<Housing>, Housing> _handler;
    private IValidator<GetById.Query<Housing>> _validator;

    [SetUp]
    public void Setup()
    {
        _housings = Housings;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Housing>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_housings.First(_ => _.Id == IdInDb)));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetById<Housing>(Cache.Housings, IdInDb)).Returns(Housings[0]);
        
        _logger = new Mock<ILogger<GetById>>();

        _handler = new GetById.QueryHandler<Housing>(_repository.Object, _cacheService.Object, _logger.Object);
        _validator = new GetById.QueryValidator<Housing>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Housing>(IdInDb);
        var expected = _housings.First(_ => _.Id == IdInDb);

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
        var query = new GetById.Query<Housing>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfHousingInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Housing>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Housings[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Housing>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ShoulThrowException()
    {
        // Arrange
        var query = new GetById.Query<Housing>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.Null);
    }
}