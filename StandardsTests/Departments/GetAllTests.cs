using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.CQRS.Departments;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Services.Interfaces;

namespace Standards.CQRS.Tests.Departments
{
    [TestFixture]
    public class GetAllTests : BaseTestFixture
    {
        private const string AbsoluteExpirationPath = "Cache:AbsoluteExpiration";
        private const string SlidingExpirationPath = "Cache:SlidingExpiration";
        
        private IList<Department> _departments;
        private List<DepartmentDto> _dtos;
        
        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private Mock<ICacheService> _cacheService;
        private Mock<IConfigService> _configService;
        
        private IRequestHandler<GetAll.Query, IList<DepartmentDto>> _handler;

        [SetUp]
        public void Setup()
        {
            _dtos = DepartmentDtos;
            _departments = Departments;

            _cancellationToken = new CancellationToken();

            _configService = new Mock<IConfigService>();
            _configService.Setup(config => config.GetValue<int>(AbsoluteExpirationPath)).Returns(5);
            _configService.Setup(config => config.GetValue<int>(SlidingExpirationPath)).Returns(2);

            _repository = new Mock<IRepository>();
            _repository.Setup(repository => repository.GetListAsync(It.IsAny<Func<IQueryable<Department>,IIncludableQueryable<Department,object>>>(), _cancellationToken))
                .Returns(Task.FromResult(_departments));

            _cacheService = new Mock<ICacheService>();
            _cacheService.Setup(cache => cache.GetOrCreateAsync(Cache.Departments, It.IsAny<Func<CancellationToken, Task<IList<Department>>>>(), _cancellationToken, It.IsAny<TimeSpan>(), It.IsAny<TimeSpan>()))
                .Returns(Task.FromResult(_departments));

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