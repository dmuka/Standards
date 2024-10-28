using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.Standards;

namespace Standards.Controllers;

[Route("api/persons/grades")]
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
        var query = new Delete.Query<Grade>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }
}