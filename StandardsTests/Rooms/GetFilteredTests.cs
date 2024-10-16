using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using Standards.Core.CQRS.Rooms;
using Standards.Core.Models;
using Standards.Core.Models.Housings;
using Standards.CQRS.Tests.Constants;
using Standards.Infrastructure.Data.Repositories.Interfaces;
using Standards.Infrastructure.Filter.Implementations;
using Standards.Infrastructure.Filter.Interfaces;
using Standards.Infrastructure.QueryableWrapper.Interface;

namespace Standards.CQRS.Tests.Rooms
{
    [TestFixture]
    public class GetFilteredTests
    {
        private const string SearchQuery = "Name"; 
        
        private QueryParameters _parameters;
        private CancellationToken _cancellationToken;
        private IList<Room> _rooms;
        private IQueryBuilder<Room> _queryBuilder;

        private Mock<IRepository> _repositoryMock;
        private Mock<IQueryBuilder<Room>> _queryBuilderMock;
        private Mock<IQueryableWrapper<Room>> _queryWrapperMock;
        private Mock<IQueryable<Room>> _queryMock;

        private IRequestHandler<GetFiltered.Query, PaginatedListModel<Room>> _handler;
        private IValidator<GetFiltered.Query> _validator;

        [SetUp]
        public void Setup()
        {
            _rooms = new List<Room>
            {
                new() {
                    Id = 1,
                    Name = "Name 1",
                    ShortName = "Short name 1",
                    Comments = "Comments 1"
                },
                new() {
                    Id = 2,
                    Name = "Name 2",
                    ShortName = "Short name 2",
                    Comments = "Comments 2"
                },
                new() {
                    Id = 3,
                    Name = "Name 3",
                    ShortName = "Short name 3",
                    Comments = "Comments 3"
                }
            };

            _parameters = new QueryParameters(searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

            _repositoryMock = new Mock<IRepository>();

            _queryBuilder = new QueryBuilder<Room>(_repositoryMock.Object);
            
            _cancellationToken = new CancellationToken();

            _queryBuilderMock = new Mock<IQueryBuilder<Room>>();

             _queryWrapperMock = new Mock<IQueryableWrapper<Room>>();
             _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Room>>(), _cancellationToken))
                   .Returns(Task.FromResult(_rooms));

            _queryMock = new Mock<IQueryable<Room>>();

            _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_rooms.AsQueryable());

            _handler = new GetFiltered.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
            _validator = new GetFiltered.QueryValidator();
        }

        [Test]
        public void Handler_IfAllDataIsValid_ReturnResult()
        {
            // Arrange
            var query = new GetFiltered.Query(_parameters);
            var expected = new PaginatedListModel<Room>(_rooms, 1, 10);

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
            var expected = new PaginatedListModel<Room>(_rooms,1, 10);

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