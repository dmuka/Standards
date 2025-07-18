using Application.UseCases.Common.GenericCRUD;
using Core.Results;
using Domain.Models.Persons;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Tests.Common;
using WebApi.Controllers;

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
            .Setup(s => s.Send(It.IsAny<GetAllBaseEntity.Query<Category>>(), CancellationToken.None))
            .ReturnsAsync(categories);

        // Act
        var result = await _controller.GetCategories();

        // Assert
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        using (Assert.EnterMultipleScope())
        {
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.Not.Null);
            Assert.That(okResult?.StatusCode, Is.EqualTo(200));
            Assert.That(okResult?.Value, Is.InstanceOf<Result<List<Category>>>());
        }
        var actualCategories = okResult?.Value as Result<List<Category>>;
        Assert.That(actualCategories, Is.Not.Null);
        Assert.That(actualCategories.Value, Is.EquivalentTo(categories));
    }

    [Test]
    public async Task GetCategory_ShouldReturnOkWithCategory()
    {
        // Arrange
        var category = Categories[0];
        _senderMock
            .Setup(s => s.Send(It.IsAny<GetById.Query<Category>>(), CancellationToken.None))
            .ReturnsAsync(category);

        // Act
        var result = await _controller.GetCategory(1);

        // Assert
        Assert.That(result, Is.InstanceOf<Ok<Category>>());
        var okResult = result as Ok<Category>;
        Assert.That(okResult, Is.Not.Null);
        Assert.That(okResult.Value, Is.EqualTo(category));
    }

    [Test]
    public async Task CreateCategory_ShouldReturnOkWithCreatedCategory()
    {
        // Arrange
        var category = Categories[0];
        _senderMock
            .Setup(s => s.Send(It.IsAny<CreateBaseEntity.Query<Category>>(), CancellationToken.None))
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
            .Setup(s => s.Send(It.IsAny<EditBaseEntity.Query<Category>>(), CancellationToken.None))
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
        _senderMock
            .Setup(s => s.Send(It.IsAny<Delete.Command<Category>>(), CancellationToken.None))
            .ReturnsAsync(1);

        // Act
        var result = await _controller.DeleteCategory(1);

        // Assert
        Assert.That(result, Is.InstanceOf<NoContent>());
    }
}