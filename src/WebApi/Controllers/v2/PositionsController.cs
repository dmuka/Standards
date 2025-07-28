using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.DTOs;
using Application.UseCases.Positions;
using Application.UseCases.Common;
using Domain.Aggregates.Positions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/PositionsController")]
public class PositionsController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetPositions()
    {
        var query = new GetAllPositions.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetPosition(Guid id)
    {
        var query = new GetPositionById.Query(new PositionId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddPosition([FromBody] PositionDto2 position)
    {
        var query = new AddPosition.Command(position);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditPosition([FromBody]PositionDto2 position)
    {
        var query = new EditPosition.Command(position);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeletePosition(Guid id)
    {
        var query = new DeletePosition.Command(new PositionId(id));

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