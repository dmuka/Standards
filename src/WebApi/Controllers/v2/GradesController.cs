using Application.Abstractions.Data.Filter.Models;
using Application.UseCases.DTOs;
using Application.UseCases.Common;
using Application.UseCases.Grades;
using Domain.Aggregates.Grades;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers.v2;

[ApiController]
[Route("api/v2/GradesController")]
public class GradeController(ISender sender) : ApiBaseController
{
    [HttpGet]
    [Route("list")]
    public async Task<IActionResult> GetGrade()
    {
        var query = new GetAllGrades.Query();

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpGet]
    [Route("{id:Guid}")]
    public async Task<IActionResult> GetGrade(Guid id)
    {
        var query = new GetGradeById.Query(new GradeId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("add")]
    public async Task<IActionResult> AddGrade([FromBody] GradeDto2 grade)
    {
        var query = new AddGrade.Command(grade);

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPut]
    [Route("edit")]
    public async Task<IActionResult> EditGrade([FromBody]GradeDto2 grade)
    {
        var query = new EditGrade.Command(grade);
            
        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpDelete]
    [Route("delete/{id:Guid}")]
    public async Task<IActionResult> DeleteGrade(Guid id)
    {
        var query = new DeleteGrade.Command(new GradeId(id));

        var result = await sender.Send(query);

        return Ok(result);
    }

    [HttpPost]
    [Route("filter")]
    public async Task<IActionResult> GetGradeByFilter([FromBody] QueryParameters parameters)
    {
        var query = new GetFiltered<Grade>.Query(parameters);

        var result = await sender.Send(query);

        return Ok(result);
    }
}