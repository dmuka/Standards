using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models;
using Standards.Core.Models.Departments;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;

namespace Standards.CQRS.Tests.BaseEntities.Places
{
    [TestFixture]
    public class GetFilteredTests : BaseTestFixture
    {
        private const string SearchQuery = "Name"; 
        
        private QueryParameters _parameters;
        private IList<Department> _departments;
        private IQueryBuilder<Department> _queryBuilder;

        private Mock<IRepository> _repositoryMock;
        private CancellationToken _cancellationToken;
        private Mock<IQueryBuilder<Department>> _queryBuilderMock;
        private Mock<IQueryableWrapper<Department>> _queryWrapperMock;
        private Mock<IQueryable<Housing>> _queryMock;

        private IRequestHandler<GetFiltered<Department>.Query, PaginatedListModel<Department>> _handler;
        private IValidator<GetFiltered<Department>.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _departments = Departments;

            _parameters = new QueryParameters(
                searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

            _repositoryMock = new Mock<IRepository>();

            _queryBuilder = new QueryBuilder<Department>(_repositoryMock.Object);
            
            _cancellationToken = new CancellationToken();

            _queryBuilderMock = new Mock<IQueryBuilder<Department>>();

             _queryWrapperMock = new Mock<IQueryableWrapper<Department>>();
             _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Department>>(), _cancellationToken))
                   .Returns(Task.FromResult(_departments));

            _queryMock = new Mock<IQueryable<Housing>>();

            _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_departments.AsQueryable());

            _handler = new GetFiltered<Department>.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
            _validator = new GetFiltered<Department>.QueryValidator();
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetFiltered<Department>.Query(_parameters);
            var expected = new PaginatedListModel<Department>(_departments, 1, 10);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new GetFiltered<Department>.Query(_parameters);
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

            var query = new GetFiltered<Department>.Query(_parameters);
            var expected = new PaginatedListModel<Department>(_departments, 1, 10);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Validator_IfParametersIsNull_ShouldHaveValidationError()
        {
            // Arrange
            _parameters = null;

            var query = new GetFiltered<Department>.Query(_parameters);
            
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

            var query = new GetFiltered<Department>.Query(_parameters);
            
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

            var query = new GetFiltered<Department>.Query(_parameters);
            
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

            var query = new GetFiltered<Department>.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.SortBy);
        }

        [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
        public void Validator_IfItemsPerPageIsZero_ShouldHaveValidationError(int itemsPerPage)
        {
            // Arrange
            _parameters.ItemsOnPage = itemsPerPage;

            var query = new GetFiltered<Department>.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.ItemsOnPage);
        }

        [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
        public void Validator_IfPageNumberIsZero_ShouldHaveValidationError(int pageNumber)
        {
            // Arrange
            _parameters.PageNumber = pageNumber;

            var query = new GetFiltered<Department>.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
        }
    }
}