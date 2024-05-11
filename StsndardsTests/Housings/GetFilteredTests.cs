using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.DTOs.Filters;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetFilteredTests
    {
        private HousingsFilterDto _filterDto;
        private CancellationToken _cancellationToken;
        private IList<HousingDto> _housings;

        private Mock<IQueryBuilder<HousingDto, HousingsFilterDto>> _queryBuilderMock;
        private Mock<IQueryableWrapper<HousingDto>> _queryWrapperMock;

        private IRequestHandler<GetFiltered.Query, PaginatedListModel<HousingDto>> _handler;
        private IValidator<GetFiltered.Query> _validator;

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

            _filterDto = new HousingsFilterDto
            {
                PageNumber = 1,
                ItemsPerPage = 10,
                SearchQuery = string.Empty
            };
            _cancellationToken = new CancellationToken();

            _queryBuilderMock = new Mock<IQueryBuilder<HousingDto, HousingsFilterDto>>();

            _queryWrapperMock = new Mock<IQueryableWrapper<HousingDto>>();
            _queryWrapperMock.Setup(m => m.ToListAsync(_housings.AsQueryable(), _cancellationToken))
                  .Returns(Task.FromResult(_housings));

            _queryBuilderMock.Setup(_ => _.SetFilter(_filterDto)).Returns(_queryBuilderMock.Object);
            _queryBuilderMock.Setup(_ => _.Filter()).Returns(_queryBuilderMock.Object);
            _queryBuilderMock.Setup(_ => _.Sort()).Returns(_queryBuilderMock.Object);
            _queryBuilderMock.Setup(_ => _.Paginate()).Returns(_queryBuilderMock.Object);
            _queryBuilderMock.Setup(_ => _.GetQuery()).Returns(_housings.AsQueryable());

            _handler = new GetFiltered.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
            _validator = new GetFiltered.QueryValidator();
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetFiltered.Query(_filterDto);
            var expected = new PaginatedListModel<HousingDto>(_housings, 1, 3, 10);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new GetFiltered.Query(_filterDto);
            _cancellationToken = new CancellationToken(true);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void Handler_IfNonvalidPagePaginateValues_ReturnResultWithDefaultValues()
        {
            // Arrange
            _filterDto.ItemsPerPage = default;
            _filterDto.PageNumber = default;

            var query = new GetFiltered.Query(_filterDto);
            var expected = new PaginatedListModel<HousingDto>(_housings, 1, 3, 10);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Validator_IfFilterDtoIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _filterDto = null;

            var query = new GetFiltered.Query(_filterDto);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Filter);
        }
    }
}