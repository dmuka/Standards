using Application.Abstractions.Data.Filter;
using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common.GenericCRUD;
using Domain.Models.Standards;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/standards/[controller]")]
[ApiController]
public class CharacteristicsController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCharacteristics()
    {
        var query = new GetAllBaseEntity.Query<Characteristic>();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetCharacteristic(int id)
    {
        var query = new GetById.Query<Characteristic>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateCharacteristic([FromBody] Characteristic characteristic)
    {
        var query = new CreateBaseEntity.Query<Characteristic>(characteristic);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditCharacteristic([FromBody] Characteristic characteristic)
    {
        var query = new EditBaseEntity.Query<Characteristic>(characteristic);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteCharacteristic(int id)
    {
        var query = new Delete.Command<Characteristic>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetCharacteristicsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Characteristic>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}