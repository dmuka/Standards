using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Sectors;
using Standards.Core.Models.Departments;
using Standards.Core.Models.DTOs;
using Standards.Infrastructure.Filter.Implementations;

namespace Standards.Controllers;

[Route("api/sectors")]
[ApiController]
public class SectorsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetSectors()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetSector(int id)
    {
        var query = new GetById.Query<Sector>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateSector([FromBody] SectorDto sector)
    {
        var query = new Create.Query(sector);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult>  EditSector([FromBody] SectorDto sector)
    {
        var query = new Edit.Query(sector);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteSector(int id)
    {
        var query = new Delete.Query<Sector>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetDepartmentsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Sector>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}