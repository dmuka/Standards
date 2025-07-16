using Application.Abstractions.Data.Filter;
using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common.GenericCRUD;
using Application.UseCases.DTOs;
using Application.UseCases.Workplaces;
using Domain.Models.Departments;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkPlacesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetWorkPlaces()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetWorkPlace(int id)
    {
        var query = new GetById.Query<Sector>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateWorkPlace([FromBody] WorkplaceDto workplace)
    {
        var query = new Create.Query(workplace);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditWorkPlace(int id, [FromBody] WorkplaceDto workplace)
    {
        var query = new Edit.Query(workplace);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetWorkplacesByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Workplace>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}