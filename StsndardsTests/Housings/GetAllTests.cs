using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace StandardsCQRSTests.Housings
{
    [TestFixture]
    public class GetAllTests
    {
        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private List<HousingDto> _housings;
        private IRequestHandler<GetAll.Query, IEnumerable<HousingDto>> _handler;

        [SetUp]
        public void Setup()
        {
            _housings = new List<HousingDto>
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
            _repository.Setup(_ => _.GetListAsync<HousingDto>(_cancellationToken)).Returns(Task.FromResult(_housings));

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
    }
}