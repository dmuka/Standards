using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class EditTests
    {
        private CancellationToken _cancellationToken;
        private int _id;
        private HousingDto _housing;

        private Mock<IRepository> _repositoryMock;

        private IRequestHandler<Edit.Query, int> _handler;
        private IValidator<Edit.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _id = 1;

            _housing = new HousingDto
            {
                Id = _id,
                Address = "Address 1",
                Name = "Name 1",
                ShortName = "Short name 1",
                FloorsCount = 1,
                Comments = "Comments 1"
            };

            _cancellationToken = new CancellationToken();

            _repositoryMock = new Mock<IRepository>();
            _repositoryMock.Setup(_ => _.GetByIdAsync<HousingDto>(_id, _cancellationToken)).Returns(Task.FromResult(_housing));
            _repositoryMock.Setup(_ => _.Update(_housing));
            _repositoryMock.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

            _handler = new Edit.QueryHandler(_repositoryMock.Object);
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
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto);
        }

        [Test]
        public void Validator_IfIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            _housing.Id = default;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Id);
        }

        [Test]
        public void Validator_IfIdNotInDB_ShouldHaveValidationError()
        {
            // Arrange
            _housing.Id = 2;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Id);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validator_IfNameIsNull_ShouldHaveValidationError(string name)
        {
            // Arrange
            _housing.Name = name;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.Name);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validator_IfShortNameIsNull_ShouldHaveValidationError(string shortName)
        {
            // Arrange
            _housing.ShortName = shortName;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.ShortName);
        }

        [TestCase(null)]
        [TestCase("")]
        public void Validator_IfAddressIsNull_ShouldHaveValidationError(string address)
        {
            // Arrange
            _housing.Address = address;

            var query = new Edit.Query(_housing);

            // Act
            var result = _validator.TestValidateAsync(query).Result;

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
            var result = _validator.TestValidateAsync(query).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.HousingDto.FloorsCount);
        }
    }
}