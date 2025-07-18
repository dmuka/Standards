using Application.Abstractions.Cache;
using Application.Abstractions.Data;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Housings;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Housings;

public class DeleteTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;

    private Housing _housing;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Delete.Command<Housing>, int> _handler;
    private IValidator<Delete.Command<Housing>> _validator;

    [SetUp]
    public void Setup()
    {
        _housing = Housings[0];

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetByIdAsync<Housing>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult<Housing?>(_housing));
        _repository.Setup(repository => repository.DeleteAsync(_housing, _cancellationToken));
        _repository.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Delete.CommandHandler<Housing>(_repository.Object, _cacheService.Object);
        _validator = new Delete.CommandValidator<Housing>(_repository.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var command = new Delete.Command<Housing>(IdInDb);

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
    
    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var command = new Delete.Command<Housing>(IdInDb);

        // Act
        _handler.Handle(command, _cancellationToken);

        // Assert
        _repository.Verify(repository => repository.GetByIdAsync<Housing>(IdInDb, _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Housing>(), _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Housings), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var command = new Delete.Command<Housing>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        var command = new Delete.Command<Housing>(id);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(housing => housing.Id);
    }
}
