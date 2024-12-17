using Application.CQRS.Common.GenericCRUD;
using Domain.Models;
using Domain.Models.Services;
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

namespace Tests.Application.CQRS.Services.Materials;

[TestFixture]
public class GetFilteredTests : BaseTestFixture
{
    private const string SearchQuery = "Name"; 
        
    private QueryParameters _parameters;
    private IList<Material> _materials;
    private IQueryBuilder<Material> _queryBuilder;

    private Mock<IRepository> _repositoryMock;
    private CancellationToken _cancellationToken;
    private Mock<IQueryBuilder<Material>> _queryBuilderMock;
    private Mock<IQueryableWrapper<Material>> _queryWrapperMock;
    private Mock<IQueryable<Material>> _queryMock;

    private IRequestHandler<GetFiltered<Material>.Query, PaginatedListModel<Material>> _handler;
    private IValidator<GetFiltered<Material>.Query> _validator;

    [SetUp]
    public void Setup()
    {
        _materials = Materials;

        _parameters = new QueryParameters(
            searchString: string.Empty, itemsOnPage: 10, pageNumber: 1);

        _repositoryMock = new Mock<IRepository>();

        _queryBuilder = new QueryBuilder<Material>(_repositoryMock.Object);
            
        _cancellationToken = CancellationToken.None;

        _queryBuilderMock = new Mock<IQueryBuilder<Material>>();

        _queryWrapperMock = new Mock<IQueryableWrapper<Material>>();
        _queryWrapperMock.Setup(m => m.ToListAsync(It.IsAny<IQueryable<Material>>(), _cancellationToken))
            .Returns(Task.FromResult(_materials));

        _queryMock = new Mock<IQueryable<Material>>();

        _queryBuilderMock.Setup(_ => _.Execute(It.IsAny<QueryParameters>())).Returns(_materials.AsQueryable());

        _handler = new GetFiltered<Material>.QueryHandler(_queryBuilderMock.Object, _queryWrapperMock.Object);
        _validator = new GetFiltered<Material>.QueryValidator();
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetFiltered<Material>.Query(_parameters);
        var expected = new PaginatedListModel<Material>(_materials, 1, 10);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        result.Should().BeEquivalentTo(expected);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetFiltered<Material>.Query(_parameters);
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

        var query = new GetFiltered<Material>.Query(_parameters);
        var expected = new PaginatedListModel<Material>(_materials, 1, 10);

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

        var query = new GetFiltered<Material>.Query(_parameters);
            
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

        var query = new GetFiltered<Material>.Query(_parameters);
            
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

        var query = new GetFiltered<Material>.Query(_parameters);
            
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

        var query = new GetFiltered<Material>.Query(_parameters);
            
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

        var query = new GetFiltered<Material>.Query(_parameters);
            
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

        var query = new GetFiltered<Material>.Query(_parameters);
            
        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Parameters.PageNumber);
    }
}