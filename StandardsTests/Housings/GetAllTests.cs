using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetAllTests : BaseTestFixture
    {
        private readonly string _absoluteExpirationPath = "Cache:AbsoluteExpiration";
        private readonly string _slidingExpirationPath = "Cache:SlidingExpiration";
        
        private IList<Housing> _housings;
        private List<HousingDto> _dtos;
        
        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private Mock<ICacheService> _cacheService;
        private Mock<IConfigService> _configService;
        
        private IRequestHandler<GetAll.Query, IList<HousingDto>> _handler;

        [SetUp]
        public void Setup()
        {
            _dtos = HousingDtos;
            _housings = Housings;

            _cancellationToken = new CancellationToken();

            _configService = new Mock<IConfigService>();
            _configService.Setup(config => config.GetValue<int>(_absoluteExpirationPath)).Returns(5);
            _configService.Setup(config => config.GetValue<int>(_slidingExpirationPath)).Returns(2);

            _repository = new Mock<IRepository>();
            _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Housing>,IIncludableQueryable<Housing,object>>>(), _cancellationToken))
                .Returns(Task.FromResult(_housings));

            _cacheService = new Mock<ICacheService>();
            _cacheService.Setup(cache => cache.GetOrCreateAsync<Housing>(Cache.Housings, It.IsAny<Func<CancellationToken, Task<IList<Housing>>>>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
                .Returns(Task.FromResult(_housings));

            _handler = new GetAll.QueryHandler(_repository.Object, _cacheService.Object, _configService.Object); 
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