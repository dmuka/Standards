using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.Models.MetrologyControl;
using Standards.Core.Models.Standards;

namespace Standards.Controllers;

[Route("api/standards/control/places")]
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
}