using Application.Abstractions.Cache;
using Application.CQRS.Common.GenericCRUD;
using Domain.Constants;
using Domain.Models.Persons;
using FluentValidation;
using FluentValidation.TestHelper;
using Infrastructure.Data.Repositories.Interfaces;
using MediatR;
using Moq;
using Tests.Common;

namespace Tests.Application.CQRS.BaseEntities.Categories;

[TestFixture]
public class GetByIdTests : BaseTestFixture
{
    private const int IdInDb = 1;
    private const int IdNotInDb = 10;
        
    private IList<Category> _categories;

    private Mock<IRepository> _repository;
    private CancellationToken _cancellationToken;
    private Mock<ICacheService> _cacheService;
        
    private IRequestHandler<GetById.Query<Category>, Category> _handler;
    private IValidator<GetById.Query<Category>> _validator;

    [SetUp]
    public void Setup()
    {
        _categories = Categories;

        _cancellationToken = CancellationToken.None;

        _repository = new Mock<IRepository>();
        _repository.Setup(_ => _.GetByIdAsync<Category>(IdInDb, _cancellationToken))
            .Returns(Task.FromResult(_categories.First(_ => _.Id == IdInDb)));

        _cacheService = new Mock<ICacheService>();
        _cacheService.Setup(cache => cache.GetById<Category>(Cache.Categories, IdInDb)).Returns(Categories[0]);

        _handler = new GetById.QueryHandler<Category>(_repository.Object, _cacheService.Object);
        _validator = new GetById.QueryValidator<Category>(_repository.Object); 
    }

    [Test]
    public void Handler_IfAllDataIsValid_ReturnResult()
    {
        // Arrange
        var query = new GetById.Query<Category>(IdInDb);
        var expected = _categories.First(_ => _.Id == IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test, TestCaseSource(nameof(ZeroOrNegativeId))]
    [TestCase(IdNotInDb)]
    public void Validator_IfIdIsInvalid_ReturnResult(int id)
    {
        // Arrange
        var query = new GetById.Query<Category>(id);

        // Act
        var result = _validator.TestValidateAsync(query, cancellationToken: _cancellationToken).Result;

        // Assert
        result.ShouldHaveValidationErrorFor(_ => _.Id);
    }

    [Test]
    public void Handler_IfCategoryInCache_ReturnCachedValue()
    {
        // Arrange
        var query = new GetById.Query<Category>(IdInDb);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(Categories[0]));
        _repository.Verify(repository => repository.GetByIdAsync<Category>(IdInDb, _cancellationToken), Times.Never);
    }

    [Test]
    public void Handler_IfCancellationTokenIsActive_ReturnNull()
    {
        // Arrange
        var query = new GetById.Query<Category>(IdInDb);
        _cancellationToken = new CancellationToken(true);

        // Act
        var result = _handler.Handle(query, _cancellationToken).Result;

        // Assert
        Assert.That(result, Is.EqualTo(null));
    }
}