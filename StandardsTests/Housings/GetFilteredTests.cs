using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetFilteredTests
    {
        private const string SearchQuery = "Name"; 
        
        private QueryParameters _parameters;
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

            _parameters = new QueryParameters(searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

            _repositoryMock = new Mock<IRepository>();

            _queryBuilder = new QueryBuilder<Housing>(_repositoryMock.Object);
            
            _cancellationToken = new CancellationToken();

            _queryBuilderMock = new Mock<IQueryBuilder<Housing>>();

             _queryWrapperMock = new Mock<IQueryableWrapper<Housing>>();
             _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Housing>>(), _cancellationToken))
                   .Returns(Task.FromResult(_housings));

            _queryMock = new Mock<IQueryable<Housing>>();

            _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_housings.AsQueryable());

            _handler = new GetFiltered.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
            _validator = new GetFiltered.QueryValidator();
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetFiltered.Query(_parameters);
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
            var query = new GetFiltered.Query(_parameters);
            _cancellationToken = new CancellationToken(true);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            Assert.That(result, Is.EqualTo(null));
        }

        [Test]
        public void Handler_IfInvalidPagePaginateValues_ReturnResultWithDefaultValues()
        {
            // Arrange
            _parameters.ItemsOnPage = default;
            _parameters.PageNumber = default;

            var query = new GetFiltered.Query(_parameters);
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
            _parameters = null;

            var query = new GetFiltered.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters);
        }

        [Test]
        public void Validator_IfSearchStringIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _parameters.SearchString = null;

            var query = new GetFiltered.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.SearchString);
        }

        [Test]
        public void Validator_IfSearchByIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _parameters.SearchBy = null;

            var query = new GetFiltered.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.SearchBy);
        }

        [Test]
        public void Validator_IfSortByIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _parameters.SortBy = null;

            var query = new GetFiltered.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.SortBy);
        }

        [TestCase(Cases.Zero)]
        [TestCase(Cases.Negative)]
        public void Validator_IfItemsPerPageIsZero_ShouldHaveValidationError(int itemsPerPage)
        {
            // Arrange
            _parameters.ItemsOnPage = itemsPerPage;

            var query = new GetFiltered.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.ItemsOnPage);
        }

        [TestCase(Cases.Zero)]
        [TestCase(Cases.Negative)]
        public void Validator_IfPageNumberIsZero_ShouldHaveValidationError(int pageNumber)
        {
            // Arrange
            _parameters.PageNumber = pageNumber;

            var query = new GetFiltered.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
        }
    }
}