using MediatR;
using Microsoft.AspNetCore.Mvc;
using Standards.Core.CQRS.Common.GenericCRUD;
using Standards.Core.CQRS.Materials;
using Standards.Core.Models.DTOs;
using Standards.Core.Models.Services;
using Standards.Infrastructure.Filter.Implementations;

namespace Standards.Controllers;

[Route("api/materials")]
[ApiController]
public class MaterialsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetMaterials()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetMaterial(int id)
    {
        var query = new GetById.Query<Material>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateMaterial([FromBody] MaterialDto material)
    {
        var query = new Create.Query(material);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditMaterial([FromBody] MaterialDto material)
    {
        var query = new Edit.Query(material);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteMaterial(int id)
    {
        var query = new Delete.Query<Material>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetMaterialsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Material>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}