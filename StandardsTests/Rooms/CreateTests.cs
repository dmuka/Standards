﻿using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Rooms
{
    [TestFixture]
    public class CreateTests
    {
        private CancellationToken _cancellationToken;
        private Room _room;
        private RoomDto _roomDto;

        private Housing _housing;
        private Sector _sector;

        private const int ValidId = 1;
        private const int IdNotInDb = 2;
        
        private Mock<IRepository> _repositoryMock;

        private IRequestHandler<Create.Query, int> _handler;
        private IValidator<Create.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _room = new Room
            {
                Id = ValidId,
                Name = "Name 1",
                ShortName = "Short name 1",
                Floor = 1,
                Height = 1,
                Length = 1,
                Width = 1,
                Comments = "Comments 1"
            };
            
            _roomDto = new RoomDto
            {
                Id = ValidId,
                Name = "Name 1",
                ShortName = "Short name 1",
                Floor = 1,
                Height = 1,
                Length = 1,
                Width = 1,
                Comments = "Comments 1",
                HousingId = ValidId,
                SectorId = ValidId
            };

            _housing = new Housing()
            {
                Id = ValidId,
                Name = "Housing"
            };

            _sector = new Sector()
            {
                Id = ValidId,
                Name = "Sector"
            };

            _cancellationToken = new CancellationToken();

            _repositoryMock = new Mock<IRepository>();
            _repositoryMock.Setup(repository => repository.AddAsync(_room, _cancellationToken));
            _repositoryMock.Setup(repository => repository.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));
            _repositoryMock.Setup(repository => repository.GetByIdAsync<Housing>(ValidId, _cancellationToken)).ReturnsAsync(_housing);
            _repositoryMock.Setup(repository => repository.GetByIdAsync<Sector>(ValidId, _cancellationToken)).ReturnsAsync(_sector);
            
            _handler = new Create.QueryHandler(_repositoryMock.Object);
            _validator = new Create.QueryValidator(_repositoryMock.Object);
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new Create.Query(_roomDto);
            var expected = 1;

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new Create.Query(_roomDto);
            _cancellationToken = new CancellationToken(true);
            _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(default(int)));

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(default(int)));
        }

        [Test]
        public void Validator_IfHousingDtoIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _roomDto = null;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validator_IfNameIsNull_ShouldHaveValidationError(string? name)
        {
            // Arrange
            _roomDto.Name = name;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room.Name);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validator_IfShortNameIsNull_ShouldHaveValidationError(string? shortName)
        {
            // Arrange
            _roomDto.ShortName = shortName;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room.ShortName);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        public void Validator_IfWidthIsInvalid_ShouldHaveValidationError(int width)
        {
            // Arrange
            _roomDto.Width = width;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room.Width);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        public void Validator_IfLengthIsInvalid_ShouldHaveValidationError(int length)
        {
            // Arrange
            _roomDto.Length = length;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room.Length);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        public void Validator_IfHeightIsInvalid_ShouldHaveValidationError(int height)
        {
            // Arrange
            _roomDto.Height = height;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room.Height);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        [TestCase(IdNotInDb)]
        public void Validator_IfHousingIdIsInvalid_ShouldHaveValidationError(int housingId)
        {
            // Arrange
            _roomDto.HousingId = housingId;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room.HousingId);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        [TestCase(IdNotInDb)]
        public void Validator_IfSectorIdIsInvalid_ShouldHaveValidationError(int sectorId)
        {
            // Arrange
            _roomDto.SectorId = sectorId;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room.SectorId);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        public void Validator_IfFloorIsInvalid_ShouldHaveValidationError(int floor)
        {
            // Arrange
            _roomDto.Floor = floor;

            var query = new Create.Query(_roomDto);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Room.Floor);
        }
    }
}