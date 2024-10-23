using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Housings;
using Standards.Core.Models.Persons;
using Standards.Infrastructure.Data.Repositories.Interfaces;

namespace Standards.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PositionsController(IRepository repository, ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetPositions()
    {
        var query = new GetAllBaseEntity<Position>.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetPosition(int id = 0)
    {
        var query = new GetById<Position>.Query(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreatePosition([FromBody] Position position)
    {
        var query = new CreateBaseEntity<Position>.Query(position);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditPosition([FromBody] Position position)
    {
        var query = new EditBaseEntity<Position>.Query(position);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeletePosition(int id)
    {
        var query = new Delete<Category>.Query(id);

        var result = await sender.Send(query);

        return Ok(result);
    }
}