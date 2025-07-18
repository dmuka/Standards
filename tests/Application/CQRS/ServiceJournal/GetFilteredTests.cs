using Application.Abstractions.Data;
using Application.Abstractions.Data.Filter;
using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common.GenericCRUD;
using Domain.Models;
using Domain.Models.Services;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Filter.Implementations;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.ServiceJournal;

[TestFixture]
public class GetFilteredTests : BaseTestFixture
{
    private const string SearchQuery = "Name"; 
    
    private QueryParameters _parameters;
    private IList<ServiceJournalItem> _serviceJournalItems;
    private IQueryBuilder<ServiceJournalItem> _queryBuilder;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<IQueryBuilder<ServiceJournalItem>> _queryBuilderMock;
    private Mock<IQueryableWrapper<ServiceJournalItem>> _queryWrapperMock;
    private Mock<IQueryable<ServiceJournalItem>> _queryMock;

    private IRequestHandler<GetFiltered<ServiceJournalItem>.Query, PaginatedListModel<ServiceJournalItem>> _handler;
    private IValidator<GetFiltered<ServiceJournalItem>.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _serviceJournalItems = ServiceJournalItems;

        _parameters = new QueryParameters(searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

        _repositoryMock = new Mock<IRepository>();

        _queryBuilder = new QueryBuilder<ServiceJournalItem>(_repositoryMock.Object);
        
        _cancellationToken = CancellationToken.None;

        _queryBuilderMock = new Mock<IQueryBuilder<ServiceJournalItem>>();

         _queryWrapperMock = new Mock<IQueryableWrapper<ServiceJournalItem>>();
         _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<ServiceJournalItem>>(), _cancellationToken))
               .Returns(Task.FromResult(_serviceJournalItems));

        _queryMock = new Mock<IQueryable<ServiceJournalItem>>();

        _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_serviceJournalItems.AsQueryable());

        _handler = new GetFiltered<ServiceJournalItem>.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
        _validator = new GetFiltered<ServiceJournalItem>.QueryValidator();
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        var expected = new PaginatedListModel<ServiceJournalItem>(_serviceJournalItems, 1, 10);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnZero()
    {
        // Arrange
        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result.Items.Count(), Is.EqualTo(0));
    }

    [Test]
    public void Handler_IfInvalidPagePaginateValues_ReturnResultWithDefaultValues()
    {
        // Arrange
        _parameters.ItemsOnPage = 0;
        _parameters.PageNumber = 0;

        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        var expected = new PaginatedListModel<ServiceJournalItem>(_serviceJournalItems,1, 10);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Validator_IfParametersIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _parameters = null!;

        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters);
    }

    [Test]
    public void Validator_IfSearchStringIsNull_ShouldHaveValidationError()
    {
        // Arrange
        _parameters.SearchString = null!;

        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<ServiceJournalItem>.Query(_parameters);
        
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
    }
}