using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetAllTests
    {
        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private List<Housing> _housings;
        private List<HousingDto> _dtos;
        private Department _department1;
        private Department _department2;
        private Department _department3;
        private Housing _housing1;
        private Housing _housing2;
        private Housing _housing3;
        private HousingDto _dto1;
        private HousingDto _dto2;
        private HousingDto _dto3;
        private Room _room1;
        private Room _room2;
        private Room _room3;
        private Room _room4;
        private Room _room5;
        private IRequestHandler<GetAll.Query, IList<HousingDto>> _handler;

        [SetUp]
        public void Setup()
        {
            
            _department1 = new Department()
            {
                Id = 1,
                Name = "DepartmentName1",
                ShortName = "DepartmentShortName1",
                Comments = "DepartmentComments1",
            };
            
            _department2 = new Department()
            {
                Id = 2,
                Name = "DepartmentName2",
                ShortName = "DepartmentShortName2",
                Comments = "DepartmentComments2",
            };
            
            _department3 = new Department()
            {
                Id = 3,
                Name = "DepartmentName3",
                ShortName = "DepartmentShortName3",
                Comments = "DepartmentComments3",
            };
            
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
            
            _housing1 = new Housing()
            {
                Id = 1,
                Address = "Address 1",
                Name = "Name 1",
                ShortName = "Short name 1",
                FloorsCount = 2,
                Departments = new List<Department>() { _department1 },
                Rooms = new List<Room>() { _room1, _room2 },
                Comments = "Comments 1"
                    
            };

            _dto1 = new HousingDto()
            {
                Id = _housing1.Id,
                Name = _housing1.Name,
                ShortName = _housing1.ShortName,
                FloorsCount = _housing1.FloorsCount,
                Address = _housing1.Address,
                Comments = _housing1.Comments,
                DepartmentIds = _housing1.Departments.Select(d => d.Id).ToList(),
                RoomIds = _housing1.Rooms.Select(r => r.Id).ToList()
            };
            
            _housing2 = new Housing()
            {
                Id = 1,
                Address = "Address 2",
                Name = "Name 2",
                ShortName = "Short name 2",
                FloorsCount = 1,
                Departments = new List<Department>() { _department2 },
                Rooms = new List<Room>() { _room3 },
                Comments = "Comments 1"
                    
            };

            _dto2 = new HousingDto()
            {
                Id = _housing2.Id,
                Name = _housing2.Name,
                ShortName = _housing2.ShortName,
                FloorsCount = _housing2.FloorsCount,
                Address = _housing2.Address,
                Comments = _housing2.Comments,
                DepartmentIds = _housing2.Departments.Select(d => d.Id).ToList(),
                RoomIds = _housing2.Rooms.Select(r => r.Id).ToList()
            };
            
            _housing3 = new Housing()
            {
                Id = 3,
                Address = "Address 3",
                Name = "Name 3",
                ShortName = "Short name 3",
                FloorsCount = 2,
                Departments = new List<Department>() { _department1, _department3 },
                Rooms = new List<Room>() { _room4, _room5 },
                Comments = "Comments 1"
                    
            };

            _dto3 = new HousingDto()
            {
                Id = _housing3.Id,
                Name = _housing3.Name,
                ShortName = _housing3.ShortName,
                FloorsCount = _housing3.FloorsCount,
                Address = _housing3.Address,
                Comments = _housing3.Comments,
                DepartmentIds = _housing3.Departments.Select(d => d.Id).ToList(),
                RoomIds = _housing3.Rooms.Select(r => r.Id).ToList()
            };
            
            _housings =
            [
                _housing1,
                _housing2,
                _housing3
            ];
            
            _dtos =
            [
                _dto1,
                _dto2,
                _dto3
            ];

            _cancellationToken = new CancellationToken();

            _repository = new Mock<IRepository>();
            _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Housing>,IIncludableQueryable<Housing,object>>>(), _cancellationToken))
                .Returns(Task.FromResult(_housings));

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
            result.Should().BeEquivalentTo(_dtos);
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
            Assert.That(result, Has.Count.EqualTo(default(int)));
        }
    }
}