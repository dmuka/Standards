using Application.UseCases.Common.GenericCRUD;
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

namespace Tests.Application.CQRS.Departments.Sectors.Workplaces;

[TestFixture]
public class GetFilteredTests : BaseTestFixture
{
    private const string SearchQuery = "Name"; 
    
    private QueryParameters _parameters;
    private IList<Workplace> _sectors;
    private IQueryBuilder<Workplace> _queryBuilder;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<IQueryBuilder<Workplace>> _queryBuilderMock;
    private Mock<IQueryableWrapper<Workplace>> _queryWrapperMock;
    private Mock<IQueryable<Workplace>> _queryMock;

    private IRequestHandler<GetFiltered<Workplace>.Query, PaginatedListModel<Workplace>> _handler;
    private IValidator<GetFiltered<Workplace>.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _sectors = Workplaces;

        _parameters = new QueryParameters(searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

        _repositoryMock = new Mock<IRepository>();

        _queryBuilder = new QueryBuilder<Workplace>(_repositoryMock.Object);
        
        _cancellationToken = CancellationToken.None;

        _queryBuilderMock = new Mock<IQueryBuilder<Workplace>>();

         _queryWrapperMock = new Mock<IQueryableWrapper<Workplace>>();
         _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Workplace>>(), _cancellationToken))
               .Returns(Task.FromResult(_sectors));

        _queryMock = new Mock<IQueryable<Workplace>>();

        _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_sectors.AsQueryable());

        _handler = new GetFiltered<Workplace>.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
        _validator = new GetFiltered<Workplace>.QueryValidator();
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetFiltered<Workplace>.Query(_parameters);
        var expected = new PaginatedListModel<Workplace>(_sectors, 1, 10);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetFiltered<Workplace>.Query(_parameters);
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

        var query = new GetFiltered<Workplace>.Query(_parameters);
        var expected = new PaginatedListModel<Workplace>(_sectors,1, 10);

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

        var query = new GetFiltered<Workplace>.Query(_parameters);
        
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

        var query = new GetFiltered<Workplace>.Query(_parameters);
        
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

        var query = new GetFiltered<Workplace>.Query(_parameters);
        
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

        var query = new GetFiltered<Workplace>.Query(_parameters);
        
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

        var query = new GetFiltered<Workplace>.Query(_parameters);
        
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

        var query = new GetFiltered<Workplace>.Query(_parameters);
        
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
    }
}