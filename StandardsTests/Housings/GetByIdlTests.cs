using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Housings;
using Standards.Core.CQRS.Sectors;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetByIdTests : BaseTestFixture
    {
        private const int IdInDb = 1;
        private const int IdNotInDb = 10;
        
        private IList<Housing> _housings;

        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private Mock<ICacheService> _cacheService;
        
        private IRequestHandler<GetById<Housing>.Query, Housing> _handler;
        private IValidator<GetById<Housing>.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _housings = Housings;

            _cancellationToken = new CancellationToken();

            _repository = new Mock<IRepository>();
            _repository.Setup(_ => _.GetByIdAsync<Housing>(IdInDb, _cancellationToken))
                .Returns(Task.FromResult(_housings.First(_ => _.Id == IdInDb)));

            _cacheService = new Mock<ICacheService>();
            _cacheService.Setup(cache => cache.GetById<Housing>(Cache.Housings, IdInDb)).Returns(Housings[0]);

            _handler = new GetById<Housing>.QueryHandler(_repository.Object, _cacheService.Object, Cache.Housings);
            _validator = new GetById<Housing>.QueryValidator(_repository.Object); 
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetById<Housing>.Query(IdInDb);
            var expected = _housings.First(_ => _.Id == IdInDb);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
        [TestCase(IdNotInDb)]
        public void Validator_IfIdIsInvalid_ReturnResult(int id)
        {
            // Arrange
            var query = new GetById<Housing>.Query(id);

            // Act
            var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Id);
        }

        [Test]
        public void Handler_IfHousingInCache_ReturnCachedValue()
        {
            // Arrange
            var query = new GetById<Housing>.Query(IdInDb);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(Housings[0]));
            _repository.Verify(repository => repository.GetByIdAsync<Housing>(IdInDb, _cancellationToken), Times.Never);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new GetById<Housing>.Query(IdInDb);
            _cancellationToken = new CancellationToken(true);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }
    }
}