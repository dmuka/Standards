﻿using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Housings;
using Standards.Core.CQRS.Sectors;
using Standards.Core.Models;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Common;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;

namespace Standards.CQRS.Tests.Housings
{
    [TestFixture]
    public class GetFilteredTests : BaseTestFixture
    {
        private const string SearchQuery = "Name"; 
        
        private QueryParameters _parameters;
        private IList<Housing> _housings;
        private IQueryBuilder<Housing> _queryBuilder;

        private Mock<IRepository> _repositoryMock;
        private CancellationToken _cancellationToken;
        private Mock<IQueryBuilder<Housing>> _queryBuilderMock;
        private Mock<IQueryableWrapper<Housing>> _queryWrapperMock;
        private Mock<IQueryable<Housing>> _queryMock;

        private IRequestHandler<GetFiltered<Housing>.Query, PaginatedListModel<Housing>> _handler;
        private IValidator<GetFiltered<Housing>.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _housings = Housings;

            _parameters = new QueryParameters(
                searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

            _repositoryMock = new Mock<IRepository>();

            _queryBuilder = new QueryBuilder<Housing>(_repositoryMock.Object);
            
            _cancellationToken = new CancellationToken();

            _queryBuilderMock = new Mock<IQueryBuilder<Housing>>();

             _queryWrapperMock = new Mock<IQueryableWrapper<Housing>>();
             _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Housing>>(), _cancellationToken))
                   .Returns(Task.FromResult(_housings));

            _queryMock = new Mock<IQueryable<Housing>>();

            _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_housings.AsQueryable());

            _handler = new GetFiltered<Housing>.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
            _validator = new GetFiltered<Housing>.QueryValidator();
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetFiltered<Housing>.Query(_parameters);
            var expected = new PaginatedListModel<Housing>(_housings, 1, 10);

            // Act
            var result = _handler.Handle(query, _cancellationToken).Result;

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [Test]
        public void Handler_IfCancellationTokenIsActive_ReturnNull()
        {
            // Arrange
            var query = new GetFiltered<Housing>.Query(_parameters);
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

            var query = new GetFiltered<Housing>.Query(_parameters);
            var expected = new PaginatedListModel<Housing>(_housings, 1, 10);

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

            var query = new GetFiltered<Housing>.Query(_parameters);
            
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

            var query = new GetFiltered<Housing>.Query(_parameters);
            
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

            var query = new GetFiltered<Housing>.Query(_parameters);
            
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

            var query = new GetFiltered<Housing>.Query(_parameters);
            
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

            var query = new GetFiltered<Housing>.Query(_parameters);
            
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

            var query = new GetFiltered<Housing>.Query(_parameters);
            
            // Act
            var result = _validator.TestValidate(query);

            // Assert
            result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
        }
    }
}