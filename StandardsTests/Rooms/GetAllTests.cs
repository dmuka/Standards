using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Rooms
{
    [TestFixture]
    public class GetAllTests
    {
        private Mock<IRepository> _repository;
        private Mock<ICacheService> _cacheService;
        private Mock<IConfigService> _configService;
        
        private CancellationToken _cancellationToken;

        private readonly string _absoluteExpirationPath = "Cache:AbsoluteExpiration";
        private readonly string _slidingExpirationPath = "Cache:SlidingExpiration";
        
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

            _configService = new Mock<IConfigService>();
            _configService.Setup(config => config.GetValue<int>(_absoluteExpirationPath)).Returns(5);
            _configService.Setup(config => config.GetValue<int>(_slidingExpirationPath)).Returns(2);

            _cacheService = new Mock<ICacheService>();
            _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Rooms, It.IsAny<Func<CancellationToken, Task<List<Room>>>>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
                .Returns(Task.FromResult(_rooms));

            _repository = new Mock<IRepository>();
            _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Room>,IIncludableQueryable<Room,object>>>(), _cancellationToken))
                .Returns(Task.FromResult(_rooms));

            _handler = new GetAll.QueryHandler(_repository.Object, _cacheService.Object, _configService.Object); 
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