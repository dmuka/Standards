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

namespace Tests.Application.CQRS.Housings.Rooms;

public class DeleteTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 2;
    private Room _sector;
        
    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;

    private IRequestHandler<Delete.Command<Room>, int> _handler;
    private IValidator<Delete.Command<Room>> _validator;

    [SetUp]
    public void Setup()
    {
        _sector = Rooms[0];

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(repository => repository.GetByIdAsync<Room>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult<Room?>(_sector));
        _repository.Setup(repository => repository.DeleteAsync(_sector, _cancellationToken));
        _repository.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

        _cacheService = new Mock<ICacheService>();

        _handler = new Delete.CommandHandler<Room>(_repository.Object, _cacheService.Object);
        _validator = new Delete.CommandValidator<Room>(_repository.Object);
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var command = new Delete.Command<Room>(IdInDb);

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(1));
    }
        
    [Test]
    public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
    {
        // Arrange
        var command = new Delete.Command<Room>(IdInDb);

        // Act
        _handler.Handle(command, _cancellationToken);

        // Assert
        _repository.Verify(repository => repository.GetByIdAsync<Room>(IdInDb, _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Room>(), _cancellationToken), Times.Once);
        _repository.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        _cacheService.Verify(cache => cache.Remove(Cache.Rooms), Times.Once);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var command = new Delete.Command<Room>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(command, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(0));
    }
        
    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfIdNotInDB_ShouldHaveValidationError(int id)
    {
        // Arrange
        var command = new Delete.Command<Room>(id);

        // Act
        var result = _validator.TestValidateAsync(command, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(room => room.Id);
    }
}