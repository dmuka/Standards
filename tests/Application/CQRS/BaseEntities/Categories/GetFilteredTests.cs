using Application.CQRS.Common.GenericCRUD;
using Domain.Models;
using Domain.Models.Persons;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using Infrastructure.Filter.Implementations;
using Infrastructure.Filter.Interfaces;
using Infrastructure.QueryableWrapper.Interface;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.BaseEntities.Categories;

[TestFixture]
public class GetFilteredTests : BaseTestFixture
{
    private const string SearchQuery = "Name"; 
        
    private QueryParameters _parameters;
    private IList<Category> _categories;
    private IQueryBuilder<Category> _queryBuilder;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<IQueryBuilder<Category>> _queryBuilderMock;
    private Mock<IQueryableWrapper<Category>> _queryWrapperMock;
    private Mock<IQueryable<Category>> _queryMock;

    private IRequestHandler<GetFiltered<Category>.Query, PaginatedListModel<Category>> _handler;
    private IValidator<GetFiltered<Category>.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _categories = Categories;

        _parameters = new QueryParameters(
            searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

        _repositoryMock = new Mock<IRepository>();

        _queryBuilder = new QueryBuilder<Category>(_repositoryMock.Object);
            
        _cancellationToken = CancellationToken.None;

        _queryBuilderMock = new Mock<IQueryBuilder<Category>>();

        _queryWrapperMock = new Mock<IQueryableWrapper<Category>>();
        _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Category>>(), _cancellationToken))
            .Returns(Task.FromResult(_categories));

        _queryMock = new Mock<IQueryable<Category>>();

        _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_categories.AsQueryable());

        _handler = new GetFiltered<Category>.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
        _validator = new GetFiltered<Category>.QueryValidator();
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetFiltered<Category>.Query(_parameters);
        var expected = new PaginatedListModel<Category>(_categories, 1, 10);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetFiltered<Category>.Query(_parameters);
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

        var query = new GetFiltered<Category>.Query(_parameters);
        var expected = new PaginatedListModel<Category>(_categories, 1, 10);

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

        var query = new GetFiltered<Category>.Query(_parameters);
            
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters);
    }

    [Test]
    public void Validator_IfSearchStringIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _parameters.SearchString = null;

        var query = new GetFiltered<Category>.Query(_parameters);
            
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.SearchString);
    }

    [Test]
    public void Validator_IfSearchByIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _parameters.SearchBy = null;

        var query = new GetFiltered<Category>.Query(_parameters);
            
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.SearchBy);
    }

    [Test]
    public void Validator_IfSortByIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _parameters.SortBy = null;

        var query = new GetFiltered<Category>.Query(_parameters);
            
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.SortBy);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfItemsPerPageIsZero_ShouldHaveValidationError(int itemsPerPage)
    {
        // Arrange
        _parameters.ItemsOnPage = itemsPerPage;

        var query = new GetFiltered<Category>.Query(_parameters);
            
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.ItemsOnPage);
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    public void Validator_IfPageNumberIsZero_ShouldHaveValidationError(int pageNumber)
    {
        // Arrange
        _parameters.PageNumber = pageNumber;

        var query = new GetFiltered<Category>.Query(_parameters);
            
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
    }
}