using MediatR;
using Moq;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Rooms
{
    [TestFixture]
    public class GetByIdTests
    {
        private int _id;
        private const int InvalidId = 10;

        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private List<Room> _rooms;
        private IRequestHandler<GetById.Query, Room> _handler;

        [SetUp]
        public void Setup()
        {
            _id = 1;

            _rooms = new List<Room>
            {
                new() {
                    Id = 1,
                    Name = "Name 1",
                    ShortName = "Short name 1",
                    Comments = "Comments 1"
                },
                new() {
                    Id = 2,
                    Name = "Name 2",
                    ShortName = "Short name 2",
                    Comments = "Comments 2"
                },
                new() {
                    Id = 3,
                    Name = "Name 3",
                    ShortName = "Short name 3",
                    Comments = "Comments 3"
                }
            };

            _cancellationToken = new CancellationToken();

            _repository = new Mock<IRepository>();
            _repository.Setup(_ => _.GetByIdAsync<Room>(_id, _cancellationToken))
                .Returns(Task.FromResult(_rooms.First(_ => _.Id == _id)));

            _handler = new GetById.QueryHandler(_repository.Object); 
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
}