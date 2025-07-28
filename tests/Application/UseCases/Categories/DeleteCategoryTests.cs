using Application.Abstractions.Data;
using Application.UseCases.Categories;
using Domain.Aggregates.Categories;
using Moq;

namespace Tests.Application.UseCases.Categories;

[TestFixture]
public class DeleteCategoryTests
{
    private const string CategoryNameValue = "Category name";
    private const string CategoryShortNameValue = "Category short name";
    
    private readonly Guid _invalidCategoryIdGuid = Guid.CreateVersion7();
    private CategoryId _invalidCategoryId;
    
    private readonly Guid _validCategoryIdGuid = Guid.CreateVersion7();
    private CategoryId _validCategoryId;
    private readonly CategoryId _categoryId = new (Guid.CreateVersion7());

    private Category _category;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private DeleteCategory.CommandHandler _handler;

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

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new DeleteCategory.CommandHandler(_categoryRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_CategoryExists_DeletesCategoryAndReturnsSuccess()
    {
        // Arrange
        var command = new DeleteCategory.Command(_validCategoryId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _categoryRepositoryMock.Verify(repository => repository.ExistsAsync(_validCategoryId, _cancellationToken), Times.Once);
            _categoryRepositoryMock.Verify(repository => repository.GetByIdAsync(_validCategoryId, _cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_CategoryDoesNotExist_ReturnsFailure()
    {
        // Arrange
        var command = new DeleteCategory.Command(_invalidCategoryId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Error, Is.EqualTo(CategoryErrors.NotFound(_invalidCategoryId)));
        }
    }
}