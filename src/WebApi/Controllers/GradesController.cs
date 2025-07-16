using Application.Abstractions.Data.Filter;
using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common.GenericCRUD;
using Domain.Models.Standards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/persons/[controller]")]
[ApiController]
public class GradesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetGrades()
    {
        var query = new GetAllBaseEntity.Query<Grade>();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetGrade(int id)
    {
        var query = new GetById.Query<Grade>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateGrade([FromBody] Grade grade)
    {
        var query = new CreateBaseEntity.Query<Grade>(grade);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditGrade([FromBody] Grade grade)
    {
        var query = new EditBaseEntity.Query<Grade>(grade);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteGrade(int id)
    {
        var query = new Delete.Command<Grade>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetGradesByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Grade>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}