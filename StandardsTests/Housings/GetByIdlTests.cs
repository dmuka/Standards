using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Create = Standards.Core.CQRS.Rooms.Create;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetByIdTests
    {
        private const int IdInDb = 1;
        private const int IdNotInDb = 10;

        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private List<HousingDto> _housings;
        private IRequestHandler<GetById.Query, HousingDto> _handler;
        private IValidator<GetById.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _housings =
            [
                new HousingDto
                {
                    Id = IdInDb,
                    Address = "Address 1",
                    Name = "Name 1",
                    ShortName = "Short name 1",
                    FloorsCount = 1,
                    Comments = "Comments 1"
                },

                new HousingDto
                {
                    Id = 2,
                    Address = "Address 2",
                    Name = "Name 2",
                    ShortName = "Short name 2",
                    FloorsCount = 2,
                    Comments = "Comments 2"
                },

                new HousingDto
                {
                    Id = 3,
                    Address = "Address 3",
                    Name = "Name 3",
                    ShortName = "Short name 3",
                    FloorsCount = 3,
                    Comments = "Comments 3"
                }
            ];

            _cancellationToken = new CancellationToken();

            _repository = new Mock<IRepository>();
            _repository.Setup(_ => _.GetByIdAsync<HousingDto>(IdInDb, _cancellationToken))
                .Returns(Task.FromResult(_housings.First(_ => _.Id == IdInDb)));

            _handler = new GetById.QueryHandler(_repository.Object);
            _validator = new GetById.QueryValidator(_repository.Object); 
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetById.Query(IdInDb);
            var expected = _housings.First(_ => _.Id == IdInDb);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(Cases.Zero)]
        [TestCase(Cases.Negative)]
        [TestCase(IdNotInDb)]
        public void Validator_IfIdIsInvalid_ReturnResult(int id)
        {
            // Arrange
            var query = new GetById.Query(id);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Id);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new GetById.Query(IdInDb);
            _cancellationToken = new CancellationToken(true);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }
    }
}