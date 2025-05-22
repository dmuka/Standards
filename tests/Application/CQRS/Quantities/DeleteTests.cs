using Application.Abstractions.Cache;
using Application.UseCases.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.Quantities;

public class DeleteTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;

    private Quantity _quantity;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Delete.Command<Quantity>, int> _handler;
    private IValidator<Delete.Command<Quantity>> _validator;

    [SetUp]
    public void Setup()
    {
        _quantity = Quantities[0];

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetByIdAsync<Quantity>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult<Quantity?>(_quantity));
        _repository.Setup(repository => repository.DeleteAsync(_quantity, _cancellationToken));
        _repository.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Delete.CommandHandler<Quantity>(_repository.Object, _cacheService.Object);
        _validator = new Delete.CommandValidator<Quantity>(_repository.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var command = new Delete.Command<Quantity>(IdInDb);

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
    
    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var command = new Delete.Command<Quantity>(IdInDb);

        // Act
        _handler.Handle(command, _cancellationToken);

        // Assert
        _repository.Verify(repository => repository.GetByIdAsync<Quantity>(IdInDb, _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Quantity>(), _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Quantities), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var command = new Delete.Command<Quantity>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdInvalid_ShouldHaveValidationError(int id)
    {
        // Arrange
        var command = new Delete.Command<Quantity>(id);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(quantity => quantity.Id);
    }
}
