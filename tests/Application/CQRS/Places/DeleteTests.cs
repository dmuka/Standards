using Application.Abstractions.Cache;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.MetrologyControl;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Places;

public class DeleteTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;

    private Place _place;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Delete.Command<Place>, int> _handler;
    private IValidator<Delete.Command<Place>> _validator;

    [SetUp]
    public void Setup()
    {
        _place = Places[0];

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetByIdAsync<Place>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult<Place?>(_place));
        _repository.Setup(repository => repository.DeleteAsync(_place, _cancellationToken));
        _repository.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Delete.CommandHandler<Place>(_repository.Object, _cacheService.Object);
        _validator = new Delete.CommandValidator<Place>(_repository.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new Delete.Command<Place>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
    
    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var query = new Delete.Command<Place>(IdInDb);

        // Act
        _handler.Handle(query, _cancellationToken);

        // Assert
        _repository.Verify(repository => repository.GetByIdAsync<Place>(IdInDb, _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Place>(), _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Places), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new Delete.Command<Place>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        var query = new Delete.Command<Place>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(place => place.Id);
    }
}
