using Application.Abstractions.Data;
using Application.UseCases.Categories;
using Application.UseCases.DTOs;
using Core.Results;
using Domain.Aggregates.Categories;
using Moq;

namespace Tests.Application.UseCases.Categories;

[TestFixture]
public class AddCategoryTests
{
    private const string CategoryNameValue = "Category name";
    private const string CategoryShortNameValue = "Category short name";
    
    private readonly CategoryId _categoryId = new (Guid.CreateVersion7());
    
    private CategoryDto2 _categoryDto;
    
    private readonly CancellationToken _cancellationToken = CancellationToken.None;
    
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private Mock<IUnitOfWork> _unitOfWorkMock;
    
    private AddCategory.CommandHandler _handler;

    [SetUp]
    public void Setup()
    {
        _categoryDto = new CategoryDto2
        {
            CategoryName = CategoryNameValue,
            CategoryShortName = CategoryShortNameValue,
            Id = _categoryId
        };
        
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        
        _handler = new AddCategory.CommandHandler(_categoryRepositoryMock.Object, _unitOfWorkMock.Object);
    }

    [Test]
    public async Task Handle_CategorySuccessfullyAdded_ReturnsSuccessResult()
    {
        // Arrange
        var command = new AddCategory.Command(_categoryDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsSuccess, Is.True);
            _categoryRepositoryMock.Verify(repository => repository.AddAsync(It.IsAny<Category>(), _cancellationToken), Times.Once);
            _unitOfWorkMock.Verify(unitOfWork => unitOfWork.CommitAsync(_cancellationToken), Times.Once);
        }
    }

    [Test]
    public async Task Handle_CategoryCreationFails_ReturnsZero()
    {
        // Arrange
        _categoryDto.CategoryName = "";
        var command = new AddCategory.Command(_categoryDto);

        // Act
        var result = await _handler.Handle(command, _cancellationToken);

        using (Assert.EnterMultipleScope())
        {
            // Assert
            Assert.That(result.IsFailure, Is.True);
            Assert.That(result.Error.Type, Is.EqualTo(ErrorType.Validation));
            Assert.That(result.Error.Description, Is.EqualTo("One or more validation errors occurred"));
        }
    }
}