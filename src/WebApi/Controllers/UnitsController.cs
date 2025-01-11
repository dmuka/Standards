using Application.CQRS.Common.GenericCRUD;
using Application.CQRS.Units;
using Domain.Models.DTOs;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Unit = Domain.Models.Unit;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UnitsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetUnits()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetUnit(int id)
    {
        var query = new GetById.Query<Unit>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateUnit([FromBody] UnitDto unit)
    {
        var query = new Create.Query(unit);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult>  EditUnit([FromBody] UnitDto unit)
    {
        var query = new Edit.Query(unit);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteUnit(int id)
    {
        var query = new Delete.Command<Unit>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetUnitsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Unit>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}