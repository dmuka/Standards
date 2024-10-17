using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class CreateTests
    {
        private CancellationToken _cancellationToken;
        private HousingDto _housing;

        private Mock<IRepository> _repositoryMock;
        private Mock<ICacheService> _cacheService;

        private IRequestHandler<Create.Query, int> _handler;
        private IValidator<Create.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _housing = new HousingDto
            {
                Id = 1,
                Address = "Address 1",
                Name = "Name 1",
                ShortName = "Short name 1",
                FloorsCount = 1,
                Comments = "Comments 1"
            };

            _cancellationToken = new CancellationToken();

            _repositoryMock = new Mock<IRepository>();
            _repositoryMock.Setup(_ => _.AddAsync(_housing, _cancellationToken));
            _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

            _cacheService = new Mock<ICacheService>();

            _handler = new Create.QueryHandler(_repositoryMock.Object, _cacheService.Object);
            _validator = new Create.QueryValidator();
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new Create.Query(_housing);
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
            var query = new Create.Query(_housing);
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

            var query = new Create.Query(_housing);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto);
        }

        [TestCase(Cases.Null)]
        [TestCase(Cases.EmptyString)]
        public void Validator_IfNameIsNull_ShouldHaveValidationError(string? name)
        {
            // Arrange
            _housing.Name = name;

            var query = new Create.Query(_housing);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Name);
        }

        [TestCase(Cases.Null)]
        [TestCase(Cases.EmptyString)]
        public void Validator_IfShortNameIsNull_ShouldHaveValidationError(string? shortName)
        {
            // Arrange
            _housing.ShortName = shortName;

            var query = new Create.Query(_housing);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.ShortName);
        }

        [TestCase(Cases.Null)]
        [TestCase(Cases.EmptyString)]
        public void Validator_IfAddressIsNull_ShouldHaveValidationError(string? address)
        {
            // Arrange
            _housing.Address = address;

            var query = new Create.Query(_housing);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Address);
        }

        [Test]
        public void Validator_IfFloorsCountIsZero_ShouldHaveValidationError()
        {
            // Arrange
            _housing.FloorsCount = default;

            var query = new Create.Query(_housing);

            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.FloorsCount);
        }
    }
}