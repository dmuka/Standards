using Application.UseCases.Common.GenericCRUD;
using Domain.Models;
using Domain.Models.MetrologyControl;
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

namespace Tests.Application.CQRS.VerificationJournal;

[TestFixture]
public class GetFilteredTests : BaseTestFixture
{
    private const string SearchQuery = "Name"; 
    
    private QueryParameters _parameters;
    private IList<VerificationJournalItem> _serviceJournalItems;
    private IQueryBuilder<VerificationJournalItem> _queryBuilder;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<IQueryBuilder<VerificationJournalItem>> _queryBuilderMock;
    private Mock<IQueryableWrapper<VerificationJournalItem>> _queryWrapperMock;
    private Mock<IQueryable<VerificationJournalItem>> _queryMock;

    private IRequestHandler<GetFiltered<VerificationJournalItem>.Query, PaginatedListModel<VerificationJournalItem>> _handler;
    private IValidator<GetFiltered<VerificationJournalItem>.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _serviceJournalItems = VerificationJournalItems;

        _parameters = new QueryParameters(searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

        _repositoryMock = new Mock<IRepository>();

        _queryBuilder = new QueryBuilder<VerificationJournalItem>(_repositoryMock.Object);
        
        _cancellationToken = CancellationToken.None;

        _queryBuilderMock = new Mock<IQueryBuilder<VerificationJournalItem>>();

         _queryWrapperMock = new Mock<IQueryableWrapper<VerificationJournalItem>>();
         _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<VerificationJournalItem>>(), _cancellationToken))
               .Returns(Task.FromResult(_serviceJournalItems));

        _queryMock = new Mock<IQueryable<VerificationJournalItem>>();

        _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_serviceJournalItems.AsQueryable());

        _handler = new GetFiltered<VerificationJournalItem>.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
        _validator = new GetFiltered<VerificationJournalItem>.QueryValidator();
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
        var expected = new PaginatedListModel<VerificationJournalItem>(_serviceJournalItems, 1, 10);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnZero()
    {
        // Arrange
        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
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

        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
        var expected = new PaginatedListModel<VerificationJournalItem>(_serviceJournalItems,1, 10);

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

        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
        
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

        var query = new GetFiltered<VerificationJournalItem>.Query(_parameters);
        
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
    }
}