using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    public class DeleteTests
    {
        private int _idInDB;
        private int _idNotInDB;
        private HousingDto _housing;
        private CancellationToken _cancellationToken;

        private Mock<IRepository> _repository;

        private IRequestHandler<Delete.Query, int> _handler;
        private IValidator<Delete.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _idInDB = 1;
            _idNotInDB = 2;

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

            _repository = new Mock<IRepository>();
            _repository.Setup(_ => _.GetByIdAsync<HousingDto>(_idInDB, _cancellationToken))
                .Returns(Task.FromResult(_housing));
            _repository.Setup(_ => _.DeleteAsync(_housing, _cancellationToken));
            _repository.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

            _handler = new Delete.QueryHandler(_repository.Object);
            _validator = new Delete.QueryValidator(_repository.Object);
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new Delete.Query(_idInDB);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new Delete.Query(_idInDB);
            _cancellationToken = new CancellationToken(true);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        [Test]
        public void Validator_IfIdNotInDB_ShouldHaveValidationError()
        {
            // Arrange
            var query = new Delete.Query(_idNotInDB);

            // Act
            var result = _validator.TestValidateAsync(query).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Id);
        }

        [Test]
        public void Validator_IfIdIsZero_ShouldHaveValidationError()
        {
            // Arrange
            var query = new Delete.Query(default);

            // Act
            var result = _validator.TestValidateAsync(query).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Id);
        }
    }
}
