using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Filters;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.Filter.Models;
using Standards.Infrastructure.QueryableWrapper.Interface;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetFilteredTests
    {
        private const string SearchQuery = "Name"; 
        
        private FilterDto _filter;
        private CancellationToken _cancellationToken;
        private IList<Housing> _housings;
        private IQueryBuilder<Housing> _queryBuilder;

        private Mock<IRepository> _repositoryMock;
        private Mock<IQueryBuilder<Housing>> _queryBuilderMock;
        private Mock<IQueryableWrapper<Housing>> _queryWrapperMock;
        private Mock<IQueryable<Housing>> _queryMock;

        private IRequestHandler<GetFiltered.Query, PaginatedListModel<Housing>> _handler;
        private IValidator<GetFiltered.Query> _validator;

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

            _filter = new FilterDto()
            {
                Page = 1,
                ItemsPerPage = 10,
                SearchQuery = string.Empty
            };

            _repositoryMock = new Mock<IRepository>();

            _queryBuilder = new QueryBuilder<Housing>(_repositoryMock.Object);
            
            _cancellationToken = new CancellationToken();

            _queryBuilderMock = new Mock<IQueryBuilder<Housing>>();

             _queryWrapperMock = new Mock<IQueryableWrapper<Housing>>();
             _queryWrapperMock.Setup(m => m.ToListAsync(_housings.AsQueryable(), _cancellationToken))
                   .Returns(Task.FromResult(_housings));

            _queryMock = new Mock<IQueryable<Housing>>();

            _queryBuilderMock.Setup(_ => _.AddFilter(It.IsAny<IFilter<Housing>>())).Returns(_queryBuilderMock.Object);
            _queryBuilderMock.Setup(_ => _.AddSorter(It.IsAny<IFilter<Housing>>())).Returns(_queryBuilderMock.Object);
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
            var query = new GetFiltered.Query(_filter);
            var expected = new PaginatedListModel<Housing>(_housings, 1, 3, 10);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new GetFiltered.Query(_filter);
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
            _filter.ItemsPerPage = default;
            _filter.Page = default;

            var query = new GetFiltered.Query(_filter);
            var expected = new PaginatedListModel<Housing>(_housings, 1, 3, 10);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Validator_IfFilterDtoIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _filter = null;

            var query = new GetFiltered.Query(_filter);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Filter);
        }

        [Test]
        public void Validator_IfSearchQueryIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _filter.SearchQuery = null;

            var query = new GetFiltered.Query(_filter);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Filter.SearchQuery);
        }

        [TestCase(Cases.Zero)]
        [TestCase(Cases.Negative)]
        public void Validator_IfItemsPerPageIsZero_ShouldHaveValidationError(int itemsPerPage)
        {
            // Arrange
            _filter.ItemsPerPage = itemsPerPage;

            var query = new GetFiltered.Query(_filter);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Filter.ItemsPerPage);
        }

        [TestCase(Cases.Zero)]
        [TestCase(Cases.Negative)]
        public void Validator_IfPageNumberIsZero_ShouldHaveValidationError(int pageNumber)
        {
            // Arrange
            _filter.Page = pageNumber;

            var query = new GetFiltered.Query(_filter);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Filter.Page);
        }
    }
}