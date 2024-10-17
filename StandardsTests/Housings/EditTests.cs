using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class EditTests
    {
        private const int ValidId = 1;
        private const int IdNotInDb = 2;
        
        private CancellationToken _cancellationToken;
        private HousingDto _housing;

        private Mock<IRepository> _repositoryMock;
        private Mock<ICacheService> _cacheService;

        private IRequestHandler<Edit.Query, int> _handler;
        private IValidator<Edit.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _housing = new HousingDto
            {
                Id = ValidId,
                Address = "Address 1",
                Name = "Name 1",
                ShortName = "Short name 1",
                FloorsCount = 1,
                Comments = "Comments 1"
            };

            _cancellationToken = new CancellationToken();

            _repositoryMock = new Mock<IRepository>();
            _repositoryMock.Setup(_ => _.GetByIdAsync<HousingDto>(ValidId, _cancellationToken)).Returns(Task.FromResult(_housing));
            _repositoryMock.Setup(_ => _.Update(_housing));
            _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

            _cacheService = new Mock<ICacheService>();
            
            _handler = new Edit.QueryHandler(_repositoryMock.Object, _cacheService.Object);
            _validator = new Edit.QueryValidator(_repositoryMock.Object);
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new Edit.Query(_housing);
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
            var query = new Edit.Query(_housing);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            _repositoryMock.Verify(repository => repository.GetQueryable<Department>(), Times.Once);
            _repositoryMock.Verify(repository => repository.GetQueryable<Room>(), Times.Once);
            _repositoryMock.Verify(repository => repository.Update(It.IsAny<Housing>()), Times.Once);
            _repositoryMock.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
            _cacheService.Verify(cache => cache.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new Edit.Query(_housing);
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
            _housing = null;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto);
        }

        [TestCase(Cases.Negative)]
        [TestCase(Cases.Zero)]
        [TestCase(IdNotInDb)]
        public void Validator_IfIdIsInvalid_ShouldHaveValidationError(int id)
        {
            // Arrange
            _housing.Id = default;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Id);
        }

        [TestCase(Cases.Null)]
        [TestCase(Cases.EmptyString)]
        public void Validator_IfNameIsNull_ShouldHaveValidationError(string? name)
        {
            // Arrange
            _housing.Name = name;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Name);
        }

        [TestCase(Cases.Null)]
        [TestCase(Cases.EmptyString)]
        public void Validator_IfShortNameIsNull_ShouldHaveValidationError(string? shortName)
        {
            // Arrange
            _housing.ShortName = shortName;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.ShortName);
        }

        [TestCase(Cases.Null)]
        [TestCase(Cases.EmptyString)]
        public void Validator_IfAddressIsNull_ShouldHaveValidationError(string? address)
        {
            // Arrange
            _housing.Address = address;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Address);
        }

        [Test]
        public void Validator_IfFloorsCountIsZero_ShouldHaveValidationError()
        {
            // Arrange
            _housing.FloorsCount = default;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.FloorsCount);
        }

        [Test]
        public void Validator_IfDepartmentIdsIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            _housing.DepartmentIds = new List<int>();

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.DepartmentIds);
        }

        [Test]
        public void Validator_IfRoomIdsIsEmpty_ShouldHaveValidationError()
        {
            // Arrange
            _housing.RoomIds = new List<int>();

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.RoomIds);
        }
    }
}