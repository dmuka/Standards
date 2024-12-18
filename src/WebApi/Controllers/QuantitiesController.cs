using Application.CQRS.Common.GenericCRUD;
using Domain.Models;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuantitiesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetQuantities()
    {
        var query = new GetAllBaseEntity.Query<Quantity>();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("")]
    public async Task<IActionResult> GetQuantity(int id)
    {
        var query = new GetById.Query<Quantity>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateQuantity([FromBody] Quantity quantity)
    {
        var query = new CreateBaseEntity.Query<Quantity>(quantity);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditQuantity([FromBody] Quantity quantity)
    {
        var query = new EditBaseEntity.Query<Quantity>(quantity);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteQuantity(int id)
    {
        var query = new Delete.Query<Quantity>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetQuantitiesByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Quantity>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}