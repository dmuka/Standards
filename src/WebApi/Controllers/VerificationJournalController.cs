using Application.UseCases.Common.GenericCRUD;
using Application.UseCases.VerificationsJournal;
using Domain.Models.DTOs;
using Domain.Models.MetrologyControl;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VerificationJournalController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetVerificationJournalItems()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetVerificationJournalItem(int id)
    {
        var query = new GetById.Query<VerificationJournalItem>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateVerificationJournalItem([FromBody] VerificationJournalItemDto standard)
    {
        var query = new Create.Command(standard);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult>  EditVerificationJournalItem([FromBody] VerificationJournalItemDto standard)
    {
        var query = new Edit.Command(standard);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteVerificationJournalItem(int id)
    {
        var query = new Delete.Command<VerificationJournalItem>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetVerificationJournalItemsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<VerificationJournalItem>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}