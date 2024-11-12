using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.Persons;

namespace Standards.Controllers;

[Route("api/persons/categories")]
[ApiController]
public class CategoriesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IResult> GetCategories()
    {
        var query = new GetAllBaseEntity.Query<Category>();

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IResult> GetCategory(int id)
    {
        var query = new GetById.Query<Category>(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IResult> CreateCategory([FromBody] Category category)
    {
        var query = new CreateBaseEntity.Query<Category>(category);

        var result = await sender.Send(query);

        return Results.Created($"/api/categories/{category.Id}", result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IResult> EditCategory([FromBody] Category category)
    {
        var query = new EditBaseEntity.Query<Category>(category);

        await sender.Send(query);

        return Results.NoContent();
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IResult> DeleteCategory(int id)
    {
        var query = new Delete.Query<Category>(id);

        await sender.Send(query);

        return Results.NoContent();
    }
}