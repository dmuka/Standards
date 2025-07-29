using Application.UseCases.Categories;
using Domain.Aggregates.Categories;
using Moq;

namespace Tests.Application.UseCases.Categories;

[TestFixture]
public class GetAllCategoriesTests
{
    private readonly CategoryId _categoryId1 = new (Guid.CreateVersion7());
    private readonly CategoryId _categoryId2 = new (Guid.CreateVersion7());
 
    private Category _category1;
    private Category _category2;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    
    private GetAllCategories.QueryHandler _handler;

    [SetUp]
    public void Setup()
    {
        _category1 = Category.Create(
            "Category name 1",
            "Category short name 1",
            _categoryId1,
            "").Value;
        _category2 = Category.Create(
            "Category name 2",
            "Category short name 2",
            _categoryId2,
            "").Value;

        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([_category1, _category2]);
        
        _handler = new GetAllCategories.QueryHandler(_categoryRepositoryMock.Object);
    }

    [Test]
    public async Task Handle_WhenCalled_ReturnsAllCategorys()
    {
        // Arrange
        var query = new GetAllCategories.Query();

        // Act
        var result = await _handler.Handle(query, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result, Has.Exactly(1).Matches<Category>(h => h.Id == _category1.Id));
            Assert.That(result, Has.Exactly(1).Matches<Category>(h => h.Id == _category2.Id));
        }
    }

    [Test]
    public async Task Handle_WhenNoCategorysExist_ReturnsEmptyList()
    {
        // Arrange
        var query = new GetAllCategories.Query();
        _categoryRepositoryMock.Setup(repository => repository.GetAllAsync(_cancellationToken))
            .ReturnsAsync([]);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result, Is.Empty);
        }
    }
}