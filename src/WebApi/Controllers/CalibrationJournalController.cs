using Application.UseCases.CalibrationsJournal;
using Application.UseCases.Common.GenericCRUD;
using Domain.Models.DTOs;
using Domain.Models.MetrologyControl;
using Infrastructure.Filter.Implementations;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CalibrationJournalController(ISender sender) : ControllerBase
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetCalibrationJournalItems()
    {
        var query = new GetAll.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetCalibrationJournalItem(int id)
    {
        var query = new GetById.Query<CalibrationJournalItem>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> CreateCalibrationJournalItem([FromBody] CalibrationJournalItemDto standard)
    {
        var query = new Create.Query(standard);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult>  EditCalibrationJournalItem([FromBody] CalibrationJournalItemDto standard)
    {
        var query = new Edit.Query(standard);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:int}")]
    public async Task<IActionResult> DeleteCalibrationJournalItem(int id)
    {
        var query = new Delete.Command<CalibrationJournalItem>(id);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetCalibrationJournalItemsByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<CalibrationJournalItem>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}