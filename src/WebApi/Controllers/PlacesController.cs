using Application.CQRS.Common.GenericCRUD;
using Domain.Models.MetrologyControl;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/standards/control/[controller]")]
[ApiController]
public class PlacesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetPlaces()
    {
        var query = new GetAllBaseEntity.Query<Place>();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetPlace(int id)
    {
        var query = new GetById.Query<Place>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreatePlace([FromBody] Place place)
    {
        var query = new CreateBaseEntity.Query<Place>(place);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditPlace([FromBody] Place place)
    {
        var query = new EditBaseEntity.Query<Place>(place);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeletePlace(int id)
    {
        var query = new Delete.Query<Place>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetPlacesByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Place>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}