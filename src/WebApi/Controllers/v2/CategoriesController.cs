using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.DTOs;
using Application.UseCases.Categories;
using Application.UseCases.Common;
using Domain.Aggregates.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/CategoriesController")]
public class CategoriesController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetAllCategories.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetCategory(Guid id)
    {
        var query = new GetCategoryById.Query(new CategoryId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddCategory([FromBody] CategoryDto2 category)
    {
        var query = new AddCategory.Command(category);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditCategory([FromBody]CategoryDto2 category)
    {
        var query = new EditCategory.Command(category);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        var query = new DeleteCategory.Command(new CategoryId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetCategoriesByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Category>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}