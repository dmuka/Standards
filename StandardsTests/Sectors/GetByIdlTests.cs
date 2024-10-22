using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Sectors;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;

    private Mock<IRepository> _repository;
    private Mock<ICacheService> _cacheMock;
    
    private CancellationToken _cancellationToken;
    
    private List<Room> _rooms;
    
    private IRequestHandler<GetById<Room>.Query, Room> _handler;
    private IValidator<GetById<Room>.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _rooms = Rooms;

        _cancellationToken = new CancellationToken();

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Room>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_rooms.First(_ => _.Id == IdInDb)));

        _cacheMock = new Mock<ICacheService>();
        _cacheMock.Setup(cache => cache.GetById<Room>(Cache.Rooms, IdInDb)).Returns(Rooms[0]);


        _handler = new GetById<Room>.QueryHandler(_repository.Object, _cacheMock.Object, Cache.Rooms); 
        _validator = new GetById<Room>.QueryValidator(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById<Room>.Query(IdInDb);
        var expected = _rooms.First(_ => _.Id == IdInDb);

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
        var query = new GetById<Room>.Query(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfRoomInCache_ReturnCachedValue()
    {
        // Arrange
        _cacheMock.Object.Create(Cache.Rooms, Rooms);
        var query = new GetById<Room>.Query(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Rooms[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Room>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById<Room>.Query(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
}