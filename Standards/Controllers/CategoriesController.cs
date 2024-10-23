using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.Constants;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Controllers;

[Route("api/persons/categories")]
[ApiController]
public class CategoriesController(IRepository repository, ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCategories()
    {
        var query = new GetAllBaseEntity.Query<Category>(Cache.Categories);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var query = new GetById.Query<Category>(id, Cache.Categories);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateCategory([FromBody] Category category)
    {
        var query = new CreateBaseEntity<Category>.Query(category);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditCategory(int id, [FromBody] Category category)
    {
        var query = new EditBaseEntity<Category>.Query(category);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var query = new Delete<Category>.Query(id);

        var result = await sender.Send(query);

        return Ok(result);
    }
}