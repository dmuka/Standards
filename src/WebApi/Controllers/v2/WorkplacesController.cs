using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.DTOs;
using Application.UseCases.Workplaces;
using Application.UseCases.Common;
using Domain.Aggregates.Workplaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/WorkplacesController")]
public class WorkplacesController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetWorkplaces()
    {
        var query = new GetAllWorkplaces.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetWorkplace(Guid id)
    {
        var query = new GetWorkplaceById.Query(new WorkplaceId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddWorkplace([FromBody] WorkplaceDto2 workplace)
    {
        var query = new AddWorkplace.Command(workplace);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditWorkplace([FromBody]WorkplaceDto2 workplace)
    {
        var query = new EditWorkplace.Command(workplace);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeleteWorkplace(Guid id)
    {
        var query = new DeleteWorkplace.Command(new WorkplaceId(id));

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