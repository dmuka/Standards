using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Core.Models.Persons;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Rooms
{
    [TestFixture]
    public class EditTests
    {
        private const int IdInDb = 1;
        private const int IdNotInDb = 2;
        
        private CancellationToken _cancellationToken;
        private RoomDto _roomDto;
        private Room _room;
        private Housing _housing;
        private Sector _sector;

        private Mock<IRepository> _repositoryMock;

        private IRequestHandler<Edit.Query, int> _handler;
        private IValidator<Edit.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _roomDto = new RoomDto
            {
                Id = IdInDb,
                Name = "Name 1",
                ShortName = "Short name 1",
                HousingId = IdInDb,
                Height = 1,
                Width = 1,
                Length = 1,
                Floor = 1,
                PersonIds = new List<int>() { 1, 2, 3 },
                WorkplaceIds = new List<int>() { 1, 2, 3 ,4 },
                Comments = "Comments 1"
            };
            
            _room = new Room
            {
                Id = IdInDb,
                Name = "Room"
            };

            _housing = new Housing()
            {
                Id = IdInDb,
                Name = "Housing"
            };

            _sector = new Sector()
            {
                Id = IdInDb,
                Name = "Sector"
            };

            _cancellationToken = new CancellationToken();

            _repositoryMock = new Mock<IRepository>();
            _repositoryMock.Setup(_ => _.GetByIdAsync<Room>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_room));
            _repositoryMock.Setup(_ => _.GetByIdAsync<Housing>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_housing));
            _repositoryMock.Setup(_ => _.GetByIdAsync<Sector>(IdInDb, _cancellationToken)).Returns(Task.FromResult(_sector));
            _repositoryMock.Setup(_ => _.Update(_room));
            _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

            _handler = new Edit.QueryHandler(_repositoryMock.Object);
            _validator = new Edit.QueryValidator(_repositoryMock.Object);
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new Edit.Query(_roomDto);
            var expected = 1;

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
        {
            // Arrange
            var query = new Edit.Query(_roomDto);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            _repositoryMock.Verify(repository => repository.GetByIdAsync<Housing>(IdInDb, _cancellationToken), Times.Once);
            _repositoryMock.Verify(repository => repository.GetQueryable<Person>(), Times.Once);
            _repositoryMock.Verify(repository => repository.GetQueryable<WorkPlace>(), Times.Once);
            _repositoryMock.Verify(repository => repository.Update(It.IsAny<Room>()), Times.Once);
            _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new Edit.Query(_roomDto);
            _cancellationToken = new CancellationToken(true);
            _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(default(int)));

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(default(int)));
        }

        [Test]
        public void Validator_IfRoomDtoIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _roomDto = null;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        [TestCase(IdNotInDb)]
        public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
        {
            // Arrange
            _roomDto.Id = id;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Id);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        [TestCase(IdNotInDb)]
        public void Validator_IfHousingIdIsInvalid_ShouldHaveValidationError(int id)
        {
            // Arrange
            _roomDto.HousingId = id;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.HousingId);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        [TestCase(IdNotInDb)]
        public void Validator_IfSectorIdIsInvalid_ShouldHaveValidationError(int id)
        {
            // Arrange
            _roomDto.SectorId = id;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.SectorId);
        }

        [TestCase(Cases.Null)]
        [TestCase(Cases.EmptyString)]
        public void Validator_IfNameIsNull_ShouldHaveValidationError(string? name)
        {
            // Arrange
            _roomDto.Name = name;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Name);
        }

        [TestCase(Cases.Null)]
        [TestCase(Cases.EmptyString)]
        public void Validator_IfShortNameIsNull_ShouldHaveValidationError(string? shortName)
        {
            // Arrange
            _roomDto.ShortName = shortName;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.ShortName);
        }

        [Test]
        public void Validator_IfPersonIdsIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            _roomDto.PersonIds = new List<int>();

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.PersonIds);
        }

        [Test]
        public void Validator_IfWorkplaceIdsIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            _roomDto.WorkplaceIds = new List<int>();

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.WorkplaceIds);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        public void Validator_IfFloorIsInvalid_ShouldHaveValidationError(int floor)
        {
            // Arrange
            _roomDto.Floor = floor;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Floor);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        public void Validator_IfLengthIsInvalid_ShouldHaveValidationError(int length)
        {
            // Arrange
            _roomDto.Length = length;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Length);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        public void Validator_IfHeightIsInvalid_ShouldHaveValidationError(int height)
        {
            // Arrange
            _roomDto.Height = height;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Height);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        public void Validator_IfWidthIsInvalid_ShouldHaveValidationError(int width)
        {
            // Arrange
            _roomDto.Width = width;

            var query = new Edit.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.RoomDto.Width);
        }
    }
}