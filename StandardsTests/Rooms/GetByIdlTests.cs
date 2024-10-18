using MediatR;
using Moq;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Rooms;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private int _id;
    private const int InvalidId = 10;

    private Mock<IRepository> _repository;
    private Mock<ICacheService> _cacheMock;
    
    private CancellationToken _cancellationToken;
    
    private List<Room> _rooms;
    private IRequestHandler<GetById.Query, Room> _handler;

    [SetUp]
    public void Setup()
    {
        _id = 1;

        _rooms = Rooms;

        _cancellationToken = new CancellationToken();

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Room>(_id, _cancellationToken))
            .Returns(Task.FromResult(_rooms.First(_ => _.Id == _id)));

        _cacheMock = new Mock<ICacheService>();

        _handler = new GetById.QueryHandler(_repository.Object, _cacheMock.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query(_id);
        var expected = _rooms.First(_ => _.Id == _id);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Handler_IfIdIsInvalid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query(InvalidId);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById.Query(_id);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
}