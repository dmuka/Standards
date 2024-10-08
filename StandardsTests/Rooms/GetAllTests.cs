using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Rooms
{
    [TestFixture]
    public class GetAllTests
    {
        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private List<Room> _rooms;
        private Room _room1;
        private Room _room2;
        private Room _room3;
        private Room _room4;
        private Room _room5;
        private IRequestHandler<GetAll.Query, IEnumerable<Room>> _handler;

        [SetUp]
        public void Setup()
        {
            _room1 = new Room()
            {
                Id = 1,
                Name = "RoomName1",
                ShortName = "RoomShortName1",
                Comments = "RoomComments1",
            };
            
            _room2 = new Room()
            {
                Id = 2,
                Name = "RoomName2",
                ShortName = "RoomShortName2",
                Comments = "RoomComments2"
            };
            
            _room3 = new Room()
            {
                Id = 3,
                Name = "RoomName3",
                ShortName = "RoomShortName3",
                Comments = "RoomComments3"
            };
            
            _room4 = new Room()
            {
                Id = 4,
                Name = "RoomName4",
                ShortName = "RoomShortName4",
                Comments = "RoomComments4"
            };
            
            _room5 = new Room()
            {
                Id = 5,
                Name = "RoomName5",
                ShortName = "RoomShortName5",
                Comments = "RoomComments5"
            };
            
            _rooms =
            [
                _room1,
                _room2,
                _room3,
                _room4,
                _room5
            ];

            _cancellationToken = new CancellationToken();

            _repository = new Mock<IRepository>();
            _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Room>,IIncludableQueryable<Room,object>>>(), _cancellationToken))
                .Returns(Task.FromResult(_rooms));

            _handler = new GetAll.QueryHandler(_repository.Object); 
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetAll.Query();

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            result.Should().BeEquivalentTo(_rooms);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnEmptyCollection()
        {
            // Arrange
            var query = new GetAll.Query();
            _cancellationToken = new CancellationToken(true);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result.Count(), Is.EqualTo(default(int)));
        }
    }
}