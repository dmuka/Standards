using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetByIdTests
    {
        private int _id;

        private Mock<IRepository> _repository;
        private CancellationToken _cancellationToken;
        private List<HousingDto> _housings;
        private IRequestHandler<GetById.Query, HousingDto> _handler;

        [SetUp]
        public void Setup()
        {
            _id = 1;

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
            _repository.Setup(_ => _.GetByIdAsync<HousingDto>(_id, _cancellationToken))
                .Returns(Task.FromResult(_housings.First(_ => _.Id == _id)));

            _handler = new GetById.QueryHandler(_repository.Object); 
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetById.Query(_id);
            var expected = _housings.First(_ => _.Id == _id);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new GetById.Query(_id);
            _cancellationToken = new CancellationToken(true);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }
    }
}