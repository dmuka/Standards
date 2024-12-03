using Application.Abstractions.Cache;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Standards;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Standards.Characteristics;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;
        
    private IList<Characteristic> _characteristics;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
        
    private IRequestHandler<GetById.Query<Characteristic>, Characteristic> _handler;
    private IValidator<GetById.Query<Characteristic>> _validator;

    [SetUp]
    public void Setup()
    {
        _characteristics = Characteristics;

        _cancellationToken = new CancellationToken();

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Characteristic>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_characteristics.First(_ => _.Id == IdInDb)));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetById<Characteristic>(Cache.Characteristics, IdInDb)).Returns(Characteristics[0]);

        _handler = new GetById.QueryHandler<Characteristic>(_repository.Object, _cacheService.Object);
        _validator = new GetById.QueryValidator<Characteristic>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Characteristic>(IdInDb);
        var expected = _characteristics.First(_ => _.Id == IdInDb);

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
        var query = new GetById.Query<Characteristic>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfCharacteristicInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Characteristic>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Characteristics[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Characteristic>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById.Query<Characteristic>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
}