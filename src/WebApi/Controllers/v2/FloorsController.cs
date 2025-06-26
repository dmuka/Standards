using Application.UseCases.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Floors;
using Domain.Aggregates.Floors;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/FloorsController")]
public class FloorsController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetFloors()
    {
        var query = new GetAllFloors.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetFloor(Guid id)
    {
        var query = new GetFloorById.Query(new FloorId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddFloor([FromBody] FloorDto floor)
    {
        var query = new AddFloor.Command(floor);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditFloor([FromBody]FloorDto floor)
    {
        var query = new EditFloor.Command(floor);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeleteFloor(Guid id)
    {
        var query = new DeleteFloor.Command(new FloorId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetFloorsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Floor>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}