using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common;
using Application.UseCases.DTOs;
using Application.UseCases.Sectors;
using Domain.Aggregates.Sectors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/SectorsController")]
public class SectorsController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetSectors()
    {
        var query = new GetAllSectors.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetSector(Guid id)
    {
        var query = new GetSectorById.Query(new SectorId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddSector([FromBody] SectorDto2 sector)
    {
        var query = new AddSector.Command(sector);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditSector([FromBody]SectorDto2 sector)
    {
        var query = new EditSector.Command(sector);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeleteSector(Guid id)
    {
        var query = new DeleteSector.Command(new SectorId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetSectorsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Sector>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}