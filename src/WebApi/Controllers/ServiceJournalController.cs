using Application.Abstractions.Data.Filter;
using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.Common.GenericCRUD;
using Application.UseCases.DTOs;
using Application.UseCases.ServicesJournal;
using Domain.Models.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceJournalController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetServiceJournalItems()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetServiceJournalItem(int id)
    {
        var query = new GetById.Query<Service>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateServiceJournalItem([FromBody] ServiceJournalItemDto serviceJournalItem)
    {
        var query = new Create.Query(serviceJournalItem);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult>  EditServiceJournalItem([FromBody] ServiceJournalItemDto serviceJournalItem)
    {
        var query = new Edit.Query(serviceJournalItem);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteServiceJournalItem(int id)
    {
        var query = new Delete.Command<Service>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetServiceJournalItemsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Service>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}