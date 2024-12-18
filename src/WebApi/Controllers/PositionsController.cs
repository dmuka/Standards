using Application.CQRS.Common.GenericCRUD;
using Domain.Models.Persons;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PositionsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetPositions()
    {
        var query = new GetAllBaseEntity.Query<Position>();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetPosition(int id)
    {
        var query = new GetById.Query<Position>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreatePosition([FromBody] Position position)
    {
        var query = new CreateBaseEntity.Query<Position>(position);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditPosition([FromBody] Position position)
    {
        var query = new EditBaseEntity.Query<Position>(position);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeletePosition(int id)
    {
        var query = new Delete.Query<Position>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetPositionsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Position>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}