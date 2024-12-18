using Application.Abstractions.Cache;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Services;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Services;

public class DeleteTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    private Service _service;
        
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Delete.Query<Service>, int> _handler;
    private IValidator<Delete.Query<Service>> _validator;

    [SetUp]
    public void Setup()
    {
        _service = Services[0];

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Service>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_service));
        _repository.Setup(_ => _.DeleteAsync(_service, _cancellationToken));
        _repository.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Delete.QueryHandler<Service>(_repository.Object, _cacheService.Object);
        _validator = new Delete.QueryValidator<Service>(_repository.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Delete.Query<Service>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
        
    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var query = new Delete.Query<Service>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        _repository.Verify(repository => repository.GetByIdAsync<Service>(IdInDb, _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Service>(), _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Services), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Delete.Query<Service>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
        
    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdNotInDB_ShouldHaveValidationError(int id)
    {
        // Arrange
        var query = new Delete.Query<Service>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }
}