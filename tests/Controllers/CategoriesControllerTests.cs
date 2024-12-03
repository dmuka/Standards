using Application.CQRS.Common.GenericCRUD;
using Domain.Models.Persons;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Moq;
using Standards.Controllers;
using Tests.Common;

namespace Tests.Controllers;

[TestFixture]
public class CategoriesControllerTests : BaseTestFixture
{
    private Mock<ISender> _senderMock;
    private CategoriesController _controller;

    [SetUp]
    public void SetUp()
    {
        _senderMock = new Mock<ISender>();
        _controller = new CategoriesController(_senderMock.Object);
    }

    [Test]
    public async Task GetCategories_ShouldReturnOkWithCategories()
    {
        // Arrange
        var categories = Categories;
        _senderMock
            .Setup(s => s.Send(It.IsAny<GetAllBaseEntity.Query<Category>>(), default))
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        Assert.That(result, Is.InstanceOf<Ok<IList<Category>>>());
        var okResult = result as Ok<IList<Category>>;
        Assert.That(okResult, Is.Not.Null);
        Assert.That((IList<Category>)okResult.Value, Is.EquivalentTo(categories));
    }

    [Test]
    public async Task GetCategory_ShouldReturnOkWithCategory()
    {
        // Arrange
        var category = Categories[0];
        _senderMock
            .Setup(s => s.Send(It.IsAny<GetById.Query<Category>>(), default))
            .ReturnsAsync(category);

        // Act
        var result = await _controller.GetCategory(1);

        // Assert
        Assert.That(result, Is.InstanceOf<Ok<Category>>());
        var okResult = result as Ok<Category>;
        Assert.That(okResult, Is.Not.Null);
        Assert.That((Category)okResult.Value, Is.EqualTo(category));
    }

    [Test]
    public async Task CreateCategory_ShouldReturnOkWithCreatedCategory()
    {
        // Arrange
        var category = Categories[0];
        _senderMock
            .Setup(s => s.Send(It.IsAny<CreateBaseEntity.Query<Category>>(), default))
            .ReturnsAsync(category.Id);

        // Act
        var result = await _controller.CreateCategory(category);

        // Assert
        Assert.That(result, Is.InstanceOf<Created<int>>());
        var okResult = result as Created<int>;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.EqualTo(category.Id));
    }

    [Test]
    public async Task EditCategory_ShouldReturnOkWithEditedCategory()
    {
        // Arrange
        var category = Categories[0];
        _senderMock
            .Setup(s => s.Send(It.IsAny<EditBaseEntity.Query<Category>>(), default))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.EditCategory(category);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContent>());
    }

    [Test]
    public async Task DeleteCategory_ShouldReturnOkWithDeletedCategory()
    {
        // Arrange
        var category = Categories[0];
        _senderMock
            .Setup(s => s.Send(It.IsAny<Delete.Query<Category>>(), default))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.DeleteCategory(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContent>());
    }
}