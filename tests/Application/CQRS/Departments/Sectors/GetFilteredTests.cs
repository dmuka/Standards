using Application.CQRS.Common.GenericCRUD;
using Domain.Models;
using Domain.Models.Departments;
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

namespace Tests.Application.CQRS.Departments.Sectors;

[TestFixture]
public class GetFilteredTests : BaseTestFixture
{
    private const string SearchQuery = "Name"; 
    
    private QueryParameters _parameters;
    private IList<Sector> _sectors;
    private IQueryBuilder<Sector> _queryBuilder;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<IQueryBuilder<Sector>> _queryBuilderMock;
    private Mock<IQueryableWrapper<Sector>> _queryWrapperMock;
    private Mock<IQueryable<Sector>> _queryMock;

    private IRequestHandler<GetFiltered<Sector>.Query, PaginatedListModel<Sector>> _handler;
    private IValidator<GetFiltered<Sector>.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _sectors = Sectors;

        _parameters = new QueryParameters(searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

        _repositoryMock = new Mock<IRepository>();

        _queryBuilder = new QueryBuilder<Sector>(_repositoryMock.Object);
        
        _cancellationToken = CancellationToken.None;

        _queryBuilderMock = new Mock<IQueryBuilder<Sector>>();

         _queryWrapperMock = new Mock<IQueryableWrapper<Sector>>();
         _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Sector>>(), _cancellationToken))
               .Returns(Task.FromResult(_sectors));

        _queryMock = new Mock<IQueryable<Sector>>();

        _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_sectors.AsQueryable());

        _handler = new GetFiltered<Sector>.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
        _validator = new GetFiltered<Sector>.QueryValidator();
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetFiltered<Sector>.Query(_parameters);
        var expected = new PaginatedListModel<Sector>(_sectors, 1, 10);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetFiltered<Sector>.Query(_parameters);
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

        var query = new GetFiltered<Sector>.Query(_parameters);
        var expected = new PaginatedListModel<Sector>(_sectors,1, 10);

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

        var query = new GetFiltered<Sector>.Query(_parameters);
        
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

        var query = new GetFiltered<Sector>.Query(_parameters);
        
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

        var query = new GetFiltered<Sector>.Query(_parameters);
        
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

        var query = new GetFiltered<Sector>.Query(_parameters);
        
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

        var query = new GetFiltered<Sector>.Query(_parameters);
        
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

        var query = new GetFiltered<Sector>.Query(_parameters);
        
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
    }
}