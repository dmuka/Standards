using Application.UseCases.Common.GenericCRUD;
using Domain.Models.Services;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Infrastructure.Results;

namespace WebApi.Controllers;

[Route("api/service/types/[controller]")]
[ApiController]
public class ServiceTypesController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IResult> GetServiceTypes()
    {
        var query = new GetAllBaseEntity.Query<ServiceType>();

        var result = await sender.Send(query);

        return result.Match(Results.Ok, CustomResults.Problem);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IResult> GetServiceType(int id)
    {
        var query = new GetById.Query<ServiceType>(id);

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IResult> CreateServiceType([FromBody] ServiceType category)
    {
        var query = new CreateBaseEntity.Query<ServiceType>(category);

        var result = await sender.Send(query);

        return Results.Created($"/api/categories/{category.Id}", result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IResult> EditServiceType([FromBody] ServiceType category)
    {
        var query = new EditBaseEntity.Query<ServiceType>(category);

        await sender.Send(query);

        return Results.NoContent();
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IResult> DeleteServiceType(int id)
    {
        var query = new Delete.Command<ServiceType>(id);

        await sender.Send(query);

        return Results.NoContent();
    }

    [HttpPost]
    [Route("filter")]
    
    public async Task<IActionResult> GetServiceTypesByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<ServiceType>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}