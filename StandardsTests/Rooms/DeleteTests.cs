using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Rooms
{
    public class DeleteTests
    {
        private const int IdInDb = 1;
        private const int IdNotInDb = 2;
        private Room _room;
        
        private CancellationToken _cancellationToken;

        private Mock<IRepository> _repository;
        private Mock<ICacheService> _cacheService;

        private IRequestHandler<Delete.Query, int> _handler;
        private IValidator<Delete.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _room = new Room
            {
                Id = IdInDb,
                Name = "Name 1",
                ShortName = "Short name 1",
                Comments = "Comments 1"
            };

            _cancellationToken = new CancellationToken();

            _repository = new Mock<IRepository>();
            _repository.Setup(_ => _.GetByIdAsync<Room>(IdInDb, _cancellationToken))
                .Returns(Task.FromResult(_room));
            _repository.Setup(_ => _.DeleteAsync(_room, _cancellationToken));
            _repository.Setup(_ => _.SaveChangesAsync(_cancellationToken)).Returns(Task.FromResult(1));

            _cacheService = new Mock<ICacheService>();

            _handler = new Delete.QueryHandler(_repository.Object, _cacheService.Object);
            _validator = new Delete.QueryValidator(_repository.Object);
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new Delete.Query(IdInDb);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(1));
        }
        
        [Test]
        public void Handler_IfAllDataIsValid_AllCallsToDbShouldBeMade()
        {
            // Arrange
            var query = new Delete.Query(IdInDb);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            _repository.Verify(repository => repository.GetByIdAsync<Room>(IdInDb, _cancellationToken), Times.Once);
            _repository.Verify(repository => repository.DeleteAsync(It.IsAny<Room>(), _cancellationToken), Times.Once);
            _repository.Verify(repository => repository.SaveChangesAsync(_cancellationToken), Times.Once);
            _cacheService.Verify(cache => cache.Remove(It.IsAny<string>()), Times.Once);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new Delete.Query(IdInDb);
            _cancellationToken = new CancellationToken(true);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }
        
        [TestCase(Cases.Zero)]
        [TestCase(Cases.Negative)]
        [TestCase(IdNotInDb)]
        public void Validator_IfIdNotInDB_ShouldHaveValidationError(int id)
        {
            // Arrange
            var query = new Delete.Query(id);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Id);
        }
    }
}
