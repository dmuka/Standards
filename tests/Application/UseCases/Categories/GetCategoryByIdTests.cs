using Application.UseCases.Categories;
using Domain.Aggregates.Categories;
using Moq;

namespace Tests.Application.UseCases.Categories;

[TestFixture]
public class GetCategoryByIdTests
{
    private const string CategoryNameValue = "Category name";
    private const string CategoryShortNameValue = "Category short name";
    
    private readonly Guid _invalidCategoryIdGuid = Guid.CreateVersion7();
    private CategoryId _invalidCategoryId;
    
    private readonly Guid _validCategoryIdGuid = Guid.CreateVersion7();
    private CategoryId _validCategoryId;

    private Category _category;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    
    private GetCategoryById.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _validCategoryId = new CategoryId(_validCategoryIdGuid);
        _invalidCategoryId = new CategoryId(_invalidCategoryIdGuid);
        
        _category = Category.Create(
            CategoryNameValue, 
            CategoryShortNameValue,
            _validCategoryId,
            "Comments").Value;        

        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryRepositoryMock.Setup(repository => repository.ExistsAsync(_validCategoryId, _cancellationToken))
            .ReturnsAsync(true);
        _categoryRepositoryMock.Setup(repository => repository.GetByIdAsync(_validCategoryId, _cancellationToken))
            .ReturnsAsync(_category);
        
        _handler = new GetCategoryById.QueryHandler(_categoryRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_CategoryExists_ReturnsCategory()
    {
        // Arrange
        var query = new GetCategoryById.Query(_validCategoryId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(_validCategoryId));
            Assert.That(result.Value.Name.Value, Is.EqualTo(CategoryNameValue));
            Assert.That(result.Value.ShortName.Value, Is.EqualTo(CategoryShortNameValue));
        }
    }

    [Test]
    public async Task Handle_CategoryDoesNotExist_ReturnsNull()
    {
        // Arrange
        var query = new GetCategoryById.Query(_invalidCategoryId);

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error, Is.EqualTo(CategoryErrors.NotFound(_invalidCategoryId)));
        }
    }
}