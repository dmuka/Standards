using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Housings;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetAllTests
    {
        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private List<Housing> _housings;
        private IRequestHandler<GetAll.Query, IEnumerable<Housing>> _handler;

        [SetUp]
        public void Setup()
        {
            _housings = new List<Housing>
            {
                new() {
                    Id = 1,
                    Address = "Address 1",
                    Name = "Name 1",
                    ShortName = "Short name 1",
                    FloorsCount = 1,
                    Comments = "Comments 1"
                },
                new() {
                    Id = 2,
                    Address = "Address 2",
                    Name = "Name 2",
                    ShortName = "Short name 2",
                    FloorsCount = 2,
                    Comments = "Comments 2"
                },
                new() {
                    Id = 3,
                    Address = "Address 3",
                    Name = "Name 3",
                    ShortName = "Short name 3",
                    FloorsCount = 3,
                    Comments = "Comments 3"
                }
            };

            _cancellationToken = new CancellationToken();

            _repository = new Mock<IRepository>();
            _repository.Setup(repository => repository.GetListAsync<Housing>(_cancellationToken)).Returns(Task.FromResult(_housings));

            _handler = new GetAll.QueryHandler(_repository.Object); 
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetAll.Query();

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EquivalentTo(_housings));
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
            Assert.That(result.Count(), Is.EqualTo(default(int)));
        }
    }
}