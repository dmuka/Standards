using Application.Abstractions.Data;
using Application.UseCases.Categories;
using Application.UseCases.DTOs;
using Domain.Aggregates.Categories;
using Moq;

namespace Tests.Application.UseCases.Categories;

[TestFixture]
public class EditCategoryTests
{
    private readonly CategoryId _categoryId = new (Guid.CreateVersion7());
    private readonly CategoryId _nonExistentCategoryId = new (Guid.CreateVersion7());
    
    private CategoryDto2 _categoryDto;
    private Category _category;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private EditCategory.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _categoryDto = new CategoryDto2
        {
            CategoryName = "Category name",
            CategoryShortName = "Category short name",
            Id = _categoryId
        };

        _category = Category.Create(
            _categoryDto.CategoryName,
            _categoryDto.CategoryShortName,
            _categoryId,
            "Comments").Value;

        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryRepositoryMock.Setup(repository => repository.ExistsAsync(_categoryId, _cancellationToken))
            .ReturnsAsync(true);
        _categoryRepositoryMock.Setup(repository => repository.GetByIdAsync(_categoryId, _cancellationToken))
            .ReturnsAsync(_category);

        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new EditCategory.CommandHandler(_categoryRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_CategoryIdNotExist_ReturnsFailure()
    {
        // Arrange
        _categoryDto.Id = _nonExistentCategoryId; 
        var command = new EditCategory.Command(_categoryDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Code, Is.EqualTo(CategoryErrors.NotFound(_nonExistentCategoryId).Code));
        }
    }

    [Test]
    public async Task Handle_CategorySuccessfullyEdited_ReturnsSuccessResult()
    {
        // Arrange
        var command = new EditCategory.Command(_categoryDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        // Assert
        Assert.That(result.IsSuccess, Is.True);
    }
}